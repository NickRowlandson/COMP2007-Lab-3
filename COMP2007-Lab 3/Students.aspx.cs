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
    public partial class Students : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // if loading page for first time, populate the student grid
            if (!IsPostBack)
            {
                // default sort column
                Session["SortColumn"] = "StudentID";
                Session["SortDirection"] = "ASC";

                // Get the student data
                this.GetStudents();
            }

        }
        /**
         * <summary>
         * This method gets the student from the DB.
         * </summary>
         * 
         * @method GetStudents
         * @return {void}
         */
        protected void GetStudents()
        {
            // connect to EF
            using (DefaultConnection db = new DefaultConnection())
            {
                string SortString = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();

                // query the students table using EF and LINQ
                var Students = (from allStudents in db.Students
                                select allStudents);

                // bind the result to the GridView
                StudentsGridview.DataSource = Students.AsQueryable().OrderBy(SortString).ToList();
                StudentsGridview.DataBind();
            }
        }

        /**
         * <summary>
         * This event handler deletes a student from the db using EF
         * </summary>
         * 
         * @method StudentGridView_RowDeleting
         * @param {object} sender
         * @param {GridViewDeletingEventArgs} e
         * @returns {void}
         * 
         */
        protected void StudentsGridview_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            // store which row was clicked
            int selectedRow = e.RowIndex;

            // get the selected student id using the grids data key collection
            int StudentID = Convert.ToInt32(StudentsGridview.DataKeys[selectedRow].Values["StudentID"]);

            // use EF to find the selected student in the DB and remove it
            using (DefaultConnection db = new DefaultConnection())
            {
                // create object of the student class and store the query string inside of it
                Student deletedStudent = (from studentRecords in db.Students
                                          where studentRecords.StudentID == StudentID
                                          select studentRecords).FirstOrDefault();

                // remove the selected student from the db
                db.Students.Remove(deletedStudent);

                // save my changes back to the db
                db.SaveChanges();

                // refresh the grid
                this.GetStudents();
            }
        }

        /**
         * <summary>
         * This event handler allows pagination to occur for the students page
         * </summary>
         * 
         * @method StudentsGridView_PageIndexChanging
         * @param {object} sender
         * @param {GridViewPageEventArgs} e
         * @return {void}
         */
        protected void StudentsGridview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // set the new page number
            StudentsGridview.PageIndex = e.NewPageIndex;

            // refresh the grid
            this.GetStudents();

        }

        /**
         * <summary>
         * This event handler sets the size of the student list
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
            StudentsGridview.PageSize = Convert.ToInt32(PageSizeDropDownList.SelectedValue);

            // refresh the grid
            this.GetStudents();
        }

        /**
         * <summary>
         * 
         * </summary>
         * 
         * @method StudentsGridview_Sorting
         * @param {object} sender
         * @param {GridViewPageEventArgs} e
         * @return {void}
         */
        protected void StudentsGridview_Sorting(object sender, GridViewSortEventArgs e)
        {
            // get the column to sort by
            Session["SortColumn"] = e.SortExpression;

            // refresh the grid
            this.GetStudents();

            // toggle the direction
            Session["SortDirection"] = Session["SortDirection"].ToString() == "ASC" ? "DESC" : "ASC";
        }

        protected void StudentsGridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (IsPostBack)
            {
                if (e.Row.RowType == DataControlRowType.Header) // if header has been clicked
                {
                    LinkButton linkButton = new LinkButton();

                    for (int index = 0; index < StudentsGridview.Columns.Count - 1; index++)
                    {
                        if(StudentsGridview.Columns[index].SortExpression == Session["SortColumn"].ToString())
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