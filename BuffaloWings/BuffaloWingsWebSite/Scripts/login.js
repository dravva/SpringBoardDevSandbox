function logintoweibo() {
    window.open('https://api.weibo.com/oauth2/authorize?client_id=528708455&scope=all&response_type=code&redirect_uri=http%3A%2F%2Fdwlc.cloudapp.net%2Fhome%2Ffromweibo', 'newwindow', 'height=400,width=800,toolbar=no,menubar=no,scrollbars=no, resizable=no,location=no, status=no');
}

function logintofacebook() {
    window.open('https://www.facebook.com/dialog/oauth?client_id=619387381484849&scope=user_status,user_likes,user_actions.video,user_actions.books,user_activities,user_videos,user_birthday,user_location,public_profile,basic_info,read_stream,export_stream,user_friends,user_photos,friends_education_history,friends_work_history,friends_interests,friends_likes,friends_birthday&redirect_uri=http%3A%2F%2Fdwlc.cloudapp.net%2Fhome%2Ffromfacebook', 'newwindow', 'height=600,width=1000,toolbar=no,menubar=no,scrollbars=no, resizable=yes,location=no, status=no');
}

function logintolinkedin() {
    window.open('https://www.linkedin.com/uas/oauth2/authorization?response_type=code&client_id=75t7rrb3xk9g17&state=dwlc&redirect_uri=http%3A%2F%2Fdwlc.cloudapp.net%2Fhome%2Ffromlinkin', 'newwindow', 'height=400,width=800,toolbar=no,menubar=no,scrollbars=no, resizable=no,location=no, status=no');
}

function logoutfromweibo() {
    $.removeCookie('weibo');
    $.removeCookie('weiboid');
    window.location.reload();
}

function logoutfromfacebook() {
    var url = "https://www.facebook.com/logout.php?next=http%3A%2F%2Fdwlc.cloudapp.net&access_token=" + $.cookie("fb");
    $.removeCookie('fb');
    $.removeCookie('fbid');
    $.removeCookie('fbavatar');
    window.location.assign(url);
}

function logoutfromlinkedin() {
    $.removeCookie('linkin');
    $.removeCookie('linkinid');
    window.location.reload();
}