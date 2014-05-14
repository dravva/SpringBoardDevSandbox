
var DldwData = {
    DataDomain: ""//"http://dldw.azurewebsites.net"
};

(function(dldwData) {
    dldwData.ShowCloseFriendsDebug = function (divName) {
        $('#showButton').click(function()
        {
            var accessToken = $("#accessToken").val();
            showFriendsOf("MostEngaging", accessToken, divName);
            showFriendsOf("MostEngaged", accessToken, divName);
            showFriendsOf("SameSchool", accessToken, divName);
        });
    };

    dldwData.ShowCloseFriends = function(divName, token) {
        showFriendsOf("MostEngaging", token, divName);
        showFriendsOf("MostEngaged", token, divName);
        showFriendsOf("SameSchool", token, divName);
        showFriendsOf("CommonInterests", token, divName);
        showFriendsOf("GatherTogether", token, divName);
        showFriendsOf("BirthdayCard", token, divName);
    };

    dldwData.ThumbUp = function(event) {
        doRating(event, "+");
    };

    dldwData.ThumbDown = function (event) {
        doRating(event, "-");
    };

    function showFriendsOf(action, accessToken, divName) {
        $.ajax(dldwData.DataDomain + "/CloseFriends/JsonP", { data: { "accessToken": accessToken, "ask": action }, crossDomain: true, dataType: "jsonp" })
            .success(function (content) {
                $(divName).append($(content.content));
            })
            .error(function() {
                $(divName).append($('<div>Error when loading data!</div>'));
        });
    }

    function showMostEngaging(accessToken, divName) {
        $.ajax(dldwData.DataDomain + "/CloseFriends/JsonP", { data: { "accessToken": accessToken, "ask": "MostEngaging" }, crossDomain: true, dataType: "jsonp" })
            .success(function (content) {
                $(divName).append($(content.content));
            })
            .error(function () {
                $(divName).append($('<div>Error when loading data!</div>'));
            });
    }

    function doRating(n, att) {
        var id = $(n.target).parent().attr('data');
        var user = getCookie("fbid");

        $.ajax({
            url: "/api/static/feedback",
            datatype: "json",
            data: {
                user: user,
                category: "Close Friends",
                item: id,
                value:id,
                feedback: att
            },
            type: "post",
            success: function(data) {
                $(n.target).siblings().hide(), $(n.target).hide();
                if (data.status == "success") {
                    var feedbackcount = "<div style=\"float:right\">" + data.up + " up, " + data.down + " down" + "</div>";
                    $(n.target).parent().append(feedbackcount);
                }
            },
            error: function() {
            }
        });
    }

    function getCookie(name) {
        if (document.cookie.length > 0) {
            var start = document.cookie.indexOf(name + "=");
            if (start != -1) {
                start = start + name.length + 1;
                var end = document.cookie.indexOf(";", start);
                if (end == -1) {
                    end = document.cookie.length;
                }
                return unescape(document.cookie.substring(start, end));
            }
        }
        return "";
    }

})(DldwData);