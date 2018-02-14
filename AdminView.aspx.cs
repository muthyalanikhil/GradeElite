using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminView : System.Web.UI.Page
{
    String connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["username"] != null && Session["role"].ToString() == "admin")
            {
                PopulateUsersGV();
            }
            else
            {
                Response.Redirect("~/Error");
            }
        }
    }

    private void PopulateUsersGV()
    {
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        try
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT id,userName,role,isBlocked FROM user";
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds);
            userListGV.DataSource = ds.Tables[0].DefaultView;
            userListGV.DataBind();
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Error");
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    protected void addUser_Click(object sender, EventArgs e)
    {
        if (addEmail.Text != null && addEmail.Text != "")
        {
            string userName = addEmail.Text;
            string password = "";
            string role = roleDD.SelectedValue.ToString();
            int isBlocked = 0;
            var pass = "gradeelite";
            if (pass != null && pass != "")
            {
                SHA1CryptoServiceProvider encrypt = new SHA1CryptoServiceProvider();
                byte[] encryptText = encrypt.ComputeHash(Encoding.Default.GetBytes(pass));
                foreach (byte tempData in encryptText)
                {
                    password = password + tempData.ToString();
                }
            }
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd;
            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO user(userName,password,role,isBlocked) VALUES(@userName,@password,@role,@isBlocked)";
                cmd.Parameters.AddWithValue("@userName", userName);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@role", role);
                cmd.Parameters.AddWithValue("@isBlocked", isBlocked);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('User not added')</script>");
                Response.Redirect("~/Error");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            PopulateUsersGV();
        }
    }

    protected void blockUser_Click(object sender, EventArgs e)
    {
        if (blockEmail.Text != null && blockEmail.Text != "")
        {
            string userName = blockEmail.Text;
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd;
            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE user SET isBlocked = 1 where userName = @userName ";
                cmd.Parameters.AddWithValue("@userName", userName);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('User not present or something went wrong. Contact Admin')</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Error");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            PopulateUsersGV();
        }
    }

    protected void unblockUser_Click(object sender, EventArgs e)
    {
        if (unblockEmail.Text != null && unblockEmail.Text != "")
        {
            string userName = unblockEmail.Text;
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd;
            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE user SET isBlocked = 0 where userName = @userName ";
                cmd.Parameters.AddWithValue("@userName", userName);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('User not present or something went wrong. Contact Admin')</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Error");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            PopulateUsersGV();
        }
    }

    protected void deleteUser_Click(object sender, EventArgs e)
    {
        if (deleteEmail.Text != null && deleteEmail.Text != "")
        {
            string userName = deleteEmail.Text;
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd;
            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM user where userName = @userName ";
                cmd.Parameters.AddWithValue("@userName", userName);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('User not present or something went wrong. Contact Admin')</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Error");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            PopulateUsersGV();
        }
    }
}