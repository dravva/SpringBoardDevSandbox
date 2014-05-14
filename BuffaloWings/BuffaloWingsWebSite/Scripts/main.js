var isSending = false;
//$.cookie("fb", "CAAIzVFzaDTEBANjnRaq4cb8CJj3BRxbNlD08TQ8I86G8GUVHif7wYLNIdsldzhk3E5ZBcAXx4jOq3WEnUSQoZAel3k059LeqGh1vrRvKhlHHhHvFB8St8ljVjhVExCgQ0jCSZCE24MlIZAZCn8drTx3xCXCZBWu9zex2flmkzXMklZCBv2XC6CqZBiRwS89Vuw0ZD");

function showCloseFriends() {
    var ftoken = $.cookie("fb");
    if (ftoken != null) {
        isSending = true;
        DldwData.ShowCloseFriends("#closeFriends", ftoken);
    }
}

function showAvatar() {
    var ftoken = $.cookie("fb");
    var fid = $.cookie("fbid");
    if (ftoken != null) {
        $("#pagetitle").append($("#templates .moment").clone().attr("id", "baseinformation"));
        $("#pagetitle #baseinformation .momentName").text("Buffalo Wings Prototype");
        $("#templates .avatar").append("<img src=\"" + $.cookie("fbavatar") + "\" alt=\"default\">");
        $("#templates .avatar").insertAfter($("#pagetitle #baseinformation .momentName"));

        var displayname = "<div class=\"displayname\">" + fid + "</div>";
        $(displayname).insertAfter($("#pagetitle #baseinformation .momentName"));
    }
}

function runAfterLoad() {    
    showAvatar();
    showCloseFriends();
    showUserStatistics();
}

function clickonup(event) {
    giveFeedback(event, "+");
}

function clickondown(event) {
    giveFeedback(event, "-");
}

function giveFeedback(event, feedback) {
    var category = "", item = "", value = "";
    var id = $.cookie("fbid");
    if (id == null) id = $.cookie("weiboid");
    if (id == null) id = "undefine";
    $.ajax({
        url: "/api/static/feedback",
        datatype: "json",
        data: { user: id, category: category, item: item, value:value,feedback: feedback },
        type: "post",
        success: function (data) {
            $(event.target).siblings().hide();
            $(event.target).hide();
            if (data.status == "success") {
                var feedbackcount = "<div style=\"float:right\">" + data.up + " up, " + data.down + " down" + "</div>";
                $(event.target).parent().append(feedbackcount);
            }
        },
        error: function () {
            isSending = false;
        }
    });
}

function showUserStatistics() {
    var ftoken = $.cookie("fb");
    if (ftoken != null) {
        isSending = true;
        UserStatisticsJs.ShowStatistics("#userStatistics", ftoken);
    }
}

window.onload = runAfterLoad;
