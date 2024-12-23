using CSharpEgitimKampi501.Dtos;
using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpEgitimKampi501
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection connection = new SqlConnection("Server=DESKTOP-7HJ6BE6\\SQLEXPRESS01;initial Catalog=EgitimKampi501Db;integrated security=true");
        private async void btnList_Click(object sender, EventArgs e)
        {
            string query = "Select * from Products";
            var values= await connection.QueryAsync<ResultProductDto>(query);
            dataGridView1.DataSource = values;
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            string query = "insert into Products (ProductName,ProductStock,ProductPrice,ProductCategory) values(@productName,@productStock,@productPrice,@productCategory)";
            var parameters = new DynamicParameters();
            parameters.Add("@productName",txtProductName.Text);
            parameters.Add("@productStock",txtProductStock.Text);
            parameters.Add("@productPrice",txtProdcutPrice.Text);
            parameters.Add("@productCategory",txtCategory.Text);
            await connection.ExecuteAsync(query,parameters);
            MessageBox.Show("Yeni kitap ekleme işlemi başarılı");
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            string query = "delete from Products where ProductId=@productId";
            var parameters = new DynamicParameters();
            parameters.Add("@productId", txtProductId.Text);
            await connection.ExecuteAsync(query,parameters);
            MessageBox.Show("Kitap silme işlemi başarılı");
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            string query = "update Products set ProductName=@productName,ProductPrice=@productPrice,ProductStock=@productStock,ProductCategory=@productCategory where ProductId=@productId";
            var parameters = new DynamicParameters();
            parameters.Add("@productName",txtProductName.Text);
            parameters.Add("@productPrice",txtProdcutPrice.Text);
            parameters.Add("@productStock",txtProductStock.Text);
            parameters.Add("@productCategory",txtCategory.Text);
            parameters.Add("@productId",txtProductId.Text);
            await connection.ExecuteAsync(query,parameters);
            MessageBox.Show("Kitap güncelleme işlemi başarılı","Güncelleme",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            string query1 = "select Count(*) from Products";
            var productTotalCount = await connection.QueryFirstOrDefaultAsync<int>(query1);
            lblTotalProductCount.Text = productTotalCount.ToString();

            string query2 = "select ProductName from Products where ProductPrice=(Select Max(ProductPrice) from Products)"; 
            var maxPRoductName=await connection.QueryFirstOrDefaultAsync<string>(query2);
            lblMaxPRiceProductName.Text = maxPRoductName.ToString();

            string query3 = "select Count(Distinct(ProductCategory)) from Products";
            var numberOfCatgories= await connection.QueryFirstOrDefaultAsync<int>(query3);
            lblCategoriesCount.Text = numberOfCatgories.ToString();
        }
    }
}
