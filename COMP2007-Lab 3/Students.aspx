﻿<%@ Page Title="Students" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Students.aspx.cs" Inherits="COMP2007_Lab_3.Students" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-offset-2 col-md-8">
                <h1>Student List</h1>
                <a href="StudentDetails.aspx" class="btn btn-success btn-sm"><i class="fa fa-plus"></i> Add Student</a>

                <div>
                    <label for="PageSizeDropDownList">Records per page:</label>
                    <asp:DropDownList ID="PageSizeDropDownList" runat="server" AutoPostBack="true" CssClass="btn btn-default bt-sm dropdown-toggle" 
                        OnSelectedIndexChanged="PageSizeDropDownList_SelectedIndexChanged">
                        <asp:ListItem Text="3" Value="3" />
                        <asp:ListItem Text="5" Value="5" />
                        <asp:ListItem Text="10" Value="10" />
                        <asp:ListItem Text="All" Value="10000" />
                    </asp:DropDownList>
                </div>
                <asp:GridView ID="StudentsGridview" runat="server" CssClass="table table-bordered table-striped table-hover" 
                    AutoGenerateColumns="false" DataKeyNames="StudentID" OnRowDeleting="StudentsGridview_RowDeleting" 
                    AllowPaging="true" PageSize="5" OnPageIndexChanging="StudentsGridview_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="StudentID" HeaderText="Student ID" Visible="true" />
                        <asp:BoundField DataField="LastName" HeaderText="Last Name" Visible="true" />
                        <asp:BoundField DataField="FirstMidName" HeaderText="First Name" Visible="true" />
                        <asp:BoundField DataField="EnrollmentDate" HeaderText="Enrollment Date" Visible="true" DataFormatString="{0:MMM dd, yyyy}"/>
                        <asp:CommandField HeaderText="Delete" DeleteText="<i class='fa fa-trash-o fa-lg'></i> Delete" ShowDeleteButton="true" ButtonType="Link" ControlStyle-CssClass="btn btn-danger btn-sm" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
