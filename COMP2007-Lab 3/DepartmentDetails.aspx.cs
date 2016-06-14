using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// Using statements required for EF DB access
using COMP2007_Lab_3.Models;
using System.Web.ModelBinding;

namespace COMP2007_Lab_3
{
    public partial class DepartmentDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!IsPostBack) && (Request.QueryString.Count > 0))
            {
                this.GetDepartment();
            }
        }

        protected void GetDepartment()
        {
            // populate the form with existing data from the database
            int DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);

            // connect to the EF DB
            using (DefaultConnection db = new DefaultConnection())
            {
                // populate a department object instance with the DepartmentID from the URL parameter
                Department updatedDepartment = (from department in db.Departments
                                          where department.DepartmentID == DepartmentID
                                          select department).FirstOrDefault();

                // map the department properties to the form controls
                if (updatedDepartment != null)
                {
                    DepartmentNameTextBox.Text = updatedDepartment.Name;
                    DepartmentBudgetTextBox.Text = Convert.ToString(updatedDepartment.Budget);
                }
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            // Redirect back to department page
            Response.Redirect("~/Departments.aspx");
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            // Use Ef to connect to the server
            using (DefaultConnection db = new DefaultConnection())
            {
                // Use the department model to create a new department object and also save a new record
                Department newDepartment = new Department();

                int DepartmentID = 0;

                if (Request.QueryString.Count > 0) // our URL has a DepartmentID in it
                {
                    // get the id from the URL
                    DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);

                    // get the current department from the EF database
                    newDepartment = (from department in db.Departments
                                  where department.DepartmentID == DepartmentID
                                     select department).FirstOrDefault();
                }


                // add data to the new department record
                newDepartment.Name = DepartmentNameTextBox.Text;
                newDepartment.Budget = Convert.ToDecimal(DepartmentBudgetTextBox.Text);

                // use LINQ to ADO.NET to add / insert new department into the database

                if (DepartmentID == 0)
                {
                    db.Departments.Add(newDepartment);
                }

                // save our changes
                db.SaveChanges();

                // redirect back to the updated departments page
                Response.Redirect("~/Departments.aspx");
            }
        }
    }
}