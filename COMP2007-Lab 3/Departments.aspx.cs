using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// using statements that are required to connnect to EF DB
using COMP2007_Lab_3.Models;
using System.Web.ModelBinding;
using System.Linq.Dynamic;

namespace COMP2007_Lab_3
{
    public partial class Departments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // if loading page for first time, populate the department grid
            if (!IsPostBack)
            {
                // default sort column
                Session["SortColumn"] = "DepartmentID";
                Session["SortDirection"] = "ASC";

                // Get the department data
                this.GetDepartments();
            }
        }

        /**
         * <summary>
         * This method gets the department from the DB.
         * </summary>
         * 
         * @method GetDepartments
         * @return {void}
         */
        protected void GetDepartments()
        {
            // connect to EF
            using (DefaultConnection db = new DefaultConnection())
            {
                string SortString = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();

                // query the departments table using EF and LINQ
                var Departments = (from allDepartments in db.Departments
                                select allDepartments);

                // bind the result to the GridView
                DepartmentGridview.DataSource = Departments.AsQueryable().OrderBy(SortString).ToList();
                DepartmentGridview.DataBind();
            }
        }

        /**
         * <summary>
         * This event handler deletes a department from the db using EF
         * </summary>
         * 
         * @method DepartmentGridView_RowDeleting
         * @param {object} sender
         * @param {GridViewDeletingEventArgs} e
         * @returns {void}
         * 
         */
        protected void DepartmentGridview_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // store which row was clicked
            int selectedRow = e.RowIndex;

            // get the selected department id using the grids data key collection
            int DepartmentID = Convert.ToInt32(DepartmentGridview.DataKeys[selectedRow].Values["DepartmentID"]);

            // use EF to find the selected department in the DB and remove it
            using (DefaultConnection db = new DefaultConnection())
            {
                // create object of the department class and store the query string inside of it
                Department deletedDepartment = (from departmentRecords in db.Departments
                                          where departmentRecords.DepartmentID == DepartmentID
                                          select departmentRecords).FirstOrDefault();

                // remove the selected department from the db
                db.Departments.Remove(deletedDepartment);

                // save my changes back to the db
                db.SaveChanges();

                // refresh the grid
                this.GetDepartments();
            }
        }

        /**
         * <summary>
         * This event handler allows pagination to occur for the departments page
         * </summary>
         * 
         * @method DepartmentGridView_PageIndexChanging
         * @param {object} sender
         * @param {GridViewPageEventArgs} e
         * @return {void}
         */
        protected void DepartmentGridview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // set the new page number
            DepartmentGridview.PageIndex = e.NewPageIndex;

            // refresh the grid
            this.GetDepartments();

        }

        /**
         * <summary>
         * This event handler sets the size of the department list
         * </summary>
         * 
         * @method PageSizeDropDownList_SelectedIndexChanged
         * @param {object} sender
         * @param {GridViewPageEventArgs} e
         * @return {void}
         */
        protected void PageSizeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // set the new list size
            DepartmentGridview.PageSize = Convert.ToInt32(PageSizeDropDownList.SelectedValue);

            // refresh the grid
            this.GetDepartments();
        }

        /**
         * <summary>
         * This event handler sets the column to sort by and sets the direction of the caret
         * </summary>
         * 
         * @method DepartmentGridview_Sorting
         * @return {void}
         */
        protected void DepartmentGridview_Sorting(object sender, GridViewSortEventArgs e)
        {
            // get the column to sort by
            Session["SortColumn"] = e.SortExpression;

            // refresh the grid
            this.GetDepartments();

            // toggle the direction
            Session["SortDirection"] = Session["SortDirection"].ToString() == "ASC" ? "DESC" : "ASC";
        }

        /**
         * <summary>
         * This event handler checks to see if the header has been clicked then sets the caret icon depending on ASC or DESC
         * </summary>
         * 
         * @method DepartmentGridview_RowDataBound
         * @return {void}
         */
        protected void DepartmentGridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (IsPostBack)
            {
                if (e.Row.RowType == DataControlRowType.Header) // if header has been clicked
                {
                    LinkButton linkButton = new LinkButton();

                    for (int index = 0; index < DepartmentGridview.Columns.Count - 1; index++)
                    {
                        if (DepartmentGridview.Columns[index].SortExpression == Session["SortColumn"].ToString())
                        {
                            if (Session["SortDirection"].ToString() == "ASC")
                            {
                                linkButton.Text = " <i class='fa fa-caret-up fa-lg'></i>";
                            }
                            else
                            {
                                linkButton.Text = " <i class='fa fa-caret-down fa-lg'></i>";
                            }

                            e.Row.Cells[index].Controls.Add(linkButton);
                        }
                    }
                }
            }
        }      

        
    }
}