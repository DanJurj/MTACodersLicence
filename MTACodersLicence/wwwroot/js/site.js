function showLoading() {
    document.getElementById("loading").style.display = 'block';
}
function showSubmitLoading() {
    document.getElementById("loadingmsg").text = "Your code is sending to the database, please wait..."
    document.getElementById("loading").style.display = 'block';
}