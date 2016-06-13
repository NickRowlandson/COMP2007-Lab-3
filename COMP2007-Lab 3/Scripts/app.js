/* Custom JS */

$(document).ready(function () {
    console.log("App started...");
    $("a.delete").click(function () {
        return confirm("Are you sure you want to delete this record?");
    })
})