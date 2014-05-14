
var UserStatisticsJs = {
   
};

(function (userStaticsJs) {
    userStaticsJs.ShowStatisticsDebug = function (divName) {
        $('#showButton').click(function () {
            var accessToken = $("#accessToken").val();
            showFlashback("Flashback", accessToken, divName);
           
        });
    };

    userStaticsJs.ShowStatistics = function (divName, token) {
        showFlashback("Flashback", token, divName);
        showStats("FBStats", token, divName);
       // showFriendsOf("SameSchool", token, divName);
       // showFriendsOf("CommonInterests", token, divName);
    };

    userStaticsJs.ThumbUp = function (event) {
        doRating(event, "+","FacebookStats");
    };

    userStaticsJs.ThumbDown = function (event) {
        doRating(event, "-", "FacebookStats");
    };

    function showFlashback(action, accessToken, divName) {
        $.ajax("/UserStatistics/JsonP", { data: { "accessToken": accessToken, "ask": action }, crossDomain: true, dataType: "jsonp" })
            .success(function (content) {
                $(divName).append($(content.content));
            })
            .error(function () {
                $(divName).append($('<div>Error when loading data!</div>'));
            });
    }

    function showStats(action,accessToken, divName) {
        $.ajax("/UserStatistics/JsonP", { data: { "accessToken": accessToken, "ask": action }, crossDomain: true, dataType: "jsonp" })
            .success(function (content) {
                $(divName).append($(content.content));
            })
            .error(function () {
                $(divName).append($('<div>Error when loading data!</div>'));
            });
    }

    function doRating(n, att, category) {
        var id = $(n.target).attr('data');
        var user = getCookie("fbid");

        $.ajax({
            url: "/api/static/feedback",
            datatype: "json",
            data: {
                user: user,
                category: category,
                item: id,
                value: id,
                feedback: att
            },
            type: "post",
            success: function (data) {
                $(n.target).siblings().hide(), $(n.target).hide();
              
                if (data.status == "success") {
                    var feedbackcount = "<div style=\"float:right\">" + data.up + " up, " + data.down + " down" + "</div>";
                    $(n.target).parent().append(feedbackcount);
                }
            },
            error: function () {
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

})(UserStatisticsJs);