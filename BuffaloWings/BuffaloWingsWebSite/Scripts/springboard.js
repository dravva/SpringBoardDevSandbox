$(document).ready(function() {
    var springboard = springboard || {};
    (function(ns) {
        var client = {};

        //private variables
        var serviceQueue = 0; //count used to wait for external feeds before rendering divs

        function twitterStringToDate(s) {
            var m = {
                Jan: 0,
                Feb: 1,
                Mar: 2,
                Apr: 3,
                May: 4,
                Jun: 5,
                Jul: 6,
                Aug: 7,
                Sep: 8,
                Oct: 9,
                Nov: 10,
                Dec: 11
            };

            var d = s.split(" ");
            var month = d[1];
            var day = d[2];
            var yr = d[5];
            var timeArr = d[3].split(":");
            var hr = timeArr[0];
            var min = timeArr[1];
            var sec = timeArr[2];
            return new Date(Date.UTC(yr, m[month], day, hr, min, sec, 0));
            //        return new Date(Date.UTC(b[0], --b[1], b[2], b[3], b[4], b[5]));
        }

        function callAPI(apiURL, domContext) {
            console.log("trying to hit API Endpoint: " + apiURL);
            $.ajax({
                type: "GET",
                url: "/api/Endpoints/Get",
                context: domContext,
                headers: {
                    "api": apiURL,
                    "Accept": "application/json"
                },


                error: function(err) {
                    console.log("error" + err);

                },
                success: function(results) {
                    console.log("results:" + results);
                    var templateDiv = document.getElementById("templates");
                    var ThirdPartyDiv = templateDiv.getElementsByClassName("ThirdParty");

                    try {
                        var list = JSON.parse(results);
                        for (var i = 0; i < 5 && !(i > list.length - 1); i++) {
                            var listItem = list[i];
                            var newCard = ThirdPartyDiv[0].cloneNode(true);
                            var action = document.createElement("li");
                            action.style.overflow = "auto";
                            var title = newCard.getElementsByClassName("cardTitle");
                            var caption = newCard.getElementsByClassName("cardCaption");
                            var thumb = newCard.getElementsByClassName("cardThumbnail");
                            title[0].innerHTML = "<a target=\"_blank\" class=\"thirdPartyLink\" href=\"" + listItem.link + "\">" + listItem.title + "</a>";
                            if (listItem.caption) {
                                caption[0].innerHTML = listItem.caption;
                            } else {
                                caption[0].style.display = "none";
                            }
                            if (listItem.thumbnail) {
                                thumb[0].style.backgroundImage = "url(" + listItem.thumbnail + ")";
                                thumb[0].style.width = "60px";
                                thumb[0].style.height = "40px";
                                thumb[0].style.backgroundSize = "cover";
                            } else {
                                thumb[0].style.display = "none";
                            }
                            //action.style.backgroundImage = "url(" + listItem.src + ")";
                            //action.style.backgroundSize = "cover";
                            action.appendChild(newCard);
                            this.appendChild(action);
                        }
                    } catch (e) {
                        console.log("error: couldn't parse results: " + e);

                    }

                }
            });
        }

        function loadFeed() {

            if (true) {
                serviceQueue--;
                var templateDiv = document.getElementById("templates");
                var page = document.getElementById("pageContent");
                var cardTemplate = templateDiv.getElementsByClassName("identity");

                for (var k = 0; k < cardData.length; k++) {
                    var startT = new Date().setHours(cardData[k].startTime);
                    var endT = new Date().setHours(cardData[k].endTime);
                    var currT = new Date();

                    console.log(startT + " -- " + currT + " -- " + endT);

                    var overlayDiv = document.getElementById("overlay");
                    for (var i = 0; i < cardData[k].topics.length; i++) {
                        var tmplt = cardData[k].topics[i].template;

                        if (tmplt.indexOf("api") > -1) {
                            var newCard = cardTemplate[0].cloneNode(true);
                            var cardTitle = newCard.getElementsByClassName("context");
                            cardTitle[0].innerHTML = cardData[k].topics[i].cardTitle;
                            var actionsList = newCard.getElementsByClassName("actions");
                            console.log("endpoint:" + cardData[k].topics[i].endpoint);
                            callAPI(cardData[k].topics[i].endpoint, actionsList[0]);
                        } else {
                            var cardTemplate = templateDiv.getElementsByClassName("card");
                            var newCard = cardTemplate[0].cloneNode(true);
                            var cardTitle = newCard.getElementsByClassName("context");
                            cardTitle[0].innerHTML = cardData[k].topics[i].cardTitle;
                            if (cardData[k].topics[i].actions.length > 0) {
                                var actionsList = newCard.getElementsByClassName("actions");

                                for (var j = 0; j < cardData[k].topics[i].actions.length; j++) {
                                    var action = document.createElement("li");
                                    if (cardData[k].topics[i].actions[j].template.length > 0) {
                                        action.className = cardData[k].topics[i].actions[j].template;
                                    }
                                    action.innerHTML = cardData[k].topics[i].actions[j].actionTitle;
                                    actionsList[0].appendChild(action);
                                }
                            }
                        }
                        page.appendChild(newCard);
                    }
                }
                overlayDiv.style.height = window.innerHeight;
            }
        }

        function searchTopic(identityObj, topic) {
            if (topic.sources.contains("outlook")) {
                var meetings = [
                    {
                        OfficialTimestamp: new Date(),
                        Source: "Outlook",
                        searchSource: "outlook",
                        searchQuery: "My Meetings",
                        height: 1284,
                        imgURL: "images/meetingStack.png"
                    }
                ]
                for (var j = 0; j < meetings.length; j++) {
                    ns.searchResults.push(meetings[j]);
                }
                topic.latestDate = new Date();
                topic.thumbnails.push("images/thumb_mentor.png");
            }
            if (topic.sources.contains("tutorial")) {
                var tutorialScreens = [
                    {
                        OfficialTimestamp: new Date(),
                        Source: "tutorial",
                        searchSource: "tutorial",
                        searchQuery: "tutorial",
                        height: 585,
                        imgURL: "images/tutorialstack.png"
                    }
                ]
                for (var j = 0; j < tutorialScreens.length; j++) {
                    ns.searchResults.push(tutorialScreens[j]);
                }
                topic.latestDate = new Date();
                topic.thumbnails.push("images/thumb_tutorials.png");
            }
            if (topic.sources.contains("twitter feed")) {

                serviceQueue++;
                $.ajax({
                    type: "GET",
                    url: "twitterProxy.aspx",
                    headers: {
                        "api": "https://api.twitter.com/1.1/statuses/home_timeline.json",
                        "aTok": identityObj.identities.twitter.accessToken,
                        "aTokSec": identityObj.identities.twitter.accessTokenSecret,
                        "twparams": "",
                        "Accept": "application/json"
                    },


                    error: function(err) {
                        serviceQueue--;
                        console.log("error" + err);
                        timeline.hose.renderItems();
                    },
                    success: function(results) {
                        serviceQueue--;
                        var twitterFeed = JSON.parse(results);
                        var SocialFeedsTopic = topic;
                        SocialFeedsTopic.latestDate = SocialFeedsTopic.latestDate || new Date(0);

                        for (var i = 0; i < twitterFeed.length; i++) {
                            var tweet = twitterFeed[i];
                            tweet.ContextList = ["Personal"];
                            tweet.Source = "Twitter";

                            var tweetDate = new Date();
                            tweet.OfficialTimestamp = twitterStringToDate(tweet.created_at);
                            if (tweet.OfficialTimestamp > SocialFeedsTopic.latestDate) SocialFeedsTopic.latestDate = tweet.OfficialTimestamp;
                            tweet.searchSource = "twitter";
                            tweet.searchQuery = "Social Feeds";
                            if (tweet.entities.media) {
                                console.log("twitter feed image found");
                                SocialFeedsTopic.thumbnails = SocialFeedsTopic.thumbnails || [];
                                if (tweet.entities.media.length > 0) {
                                    SocialFeedsTopic.thumbnails.push(tweet.entities.media[0].media_url);
                                }
                            }
                            ns.searchResults.push(tweet);
                        }
                        timeline.hose.renderItems();
                    }
                });
            }
            if (topic.sources.contains("facebook feed") && identityObj.identities.facebook.accessToken) {
                serviceQueue++;
                $.ajax({
                    type: "GET",
                    url: "fbProxy.aspx",
                    headers: {
                        "api": "https://graph.facebook.com/me?fields=home&access_token=" + identityObj.identities.facebook.accessToken,
                        "access_token": identityObj.identities.facebook.accessToken,
                        "Accept": "application/json"
                    },

                    error: function(err) {
                        serviceQueue--;
                        timeline.hose.renderItems();
                        console.log("error" + err);
                    },
                    success: function(results) {
                        serviceQueue--;
                        var SocialFeedsTopic = topic;
                        // var templatesDiv = document.getElementById("templates");
                        var newsFeed = JSON.parse(results);
                        SocialFeedsTopic.latestDate = SocialFeedsTopic.latestDate || new Date(0);
                        for (var i = 0; i < newsFeed.home.data.length; i++) {
                            var post = newsFeed.home.data[i];
                            post.ContextList = ["Personal"];
                            post.Source = "Facebook";
                            post.searchSource = "facebook";
                            post.searchQuery = "Social Feeds";
                            if (post.picture) {
                                //need to add thumbnails to social feeds topic
                                SocialFeedsTopic.thumbnails = SocialFeedsTopic.thumbnails || [];
                                SocialFeedsTopic.thumbnails.push(post.picture);

                            }
                            var d = new Date();
                            post.OfficialTimestamp = d.setISO8601(post.created_time);
                            if (post.OfficialTimestamp > SocialFeedsTopic.latestDate) SocialFeedsTopic.latestDate = post.OfficialTimestamp;
                            ns.searchResults.push(post);
                        }
                        timeline.hose.renderItems();
                    }
                });


            }
            if (topic.sources.contains("facebook") && identityObj.identities.facebook.accessToken) {
                serviceQueue++;
                $.ajax({
                    context: ns,
                    type: "GET",
                    url: "fbProxy.aspx",
                    headers: {
                        "api": "https://graph.facebook.com/search?q=" + Url.encode(topic.query) + "&type=post&access_token=" + identityObj.identities.facebook.accessToken,
                        "aTok": identityObj.identities.facebook.accessToken,
                        "Accept": "application/json"
                    },
                    error: function(err) {
                        serviceQueue--;
                        timeline.hose.renderItems();
                        console.log("facebook error " + err.statusText);
                    },
                    success: function(results) {
                        serviceQueue--;
                        var facebookFeed = JSON.parse(results);
                        facebookFeed.source = "facebook";
                        facebookFeed.OfficialTimestamp = new Date();
                        this.searchResults = this.searchResults || [];
                        topic.latestDate = topic.latestDate || new Date(0);
                        var posts = facebookFeed.data;
                        topic.facebookCount = posts.length;
                        topic.thumbnails = topic.thumbnails || [];
                        for (var i = 0; i < posts.length; i++) {
                            var post = posts[i];
                            var d = new Date();
                            post.OfficialTimestamp = d.setISO8601(post.created_time);
                            if (post.OfficialTimestamp > topic.latestDate) topic.latestDate = post.OfficialTimestamp;
                            //console.log("facebook result");
                            if (post.picture) {
                                //need to add thumbnails to social feeds topic
                                topic.thumbnails.push(post.picture);

                            }
                            post.searchSource = "facebook";
                            post.searchQuery = topic.query;
                            this.searchResults.push(post);
                        }
                        timeline.hose.renderItems();
                        console.log("got facebook");
                    }
                });
            }
            if (topic.sources.contains("twitter")) {
                serviceQueue++;
                $.ajax({
                    context: ns,
                    type: "GET",
                    url: "twitterProxy.aspx",
                    headers: {
                        "api": "https://api.twitter.com/1.1/search/tweets.json",
                        "twparams": topic.query,
                        "aTok": identityObj.identities.twitter.accessToken,
                        "aTokSec": identityObj.identities.twitter.accessTokenSecret,
                        "Accept": "application/json"
                    },
                    error: function(err) {
                        serviceQueue--;
                        timeline.hose.renderItems();
                        console.log("twitter error " + err.statusText);
                    },
                    success: function(results) {
                        serviceQueue--;
                        var twitterFeed = JSON.parse(results);
                        twitterFeed.source = "twitter";
                        twitterFeed.OfficialTimestamp = new Date();
                        this.searchResults = this.searchResults || [];
                        topic.latestDate = topic.latestDate || new Date(0);
                        var tweets = twitterFeed.statuses;
                        topic.tweetCount = tweets.length;
                        for (var i = 0; i < tweets.length; i++) {
                            var tweet = tweets[i];
                            if (tweet.entities.media) {
                                topic.thumbnails = topic.thumbnails || [];
                                if (tweet.entities.media.length > 0) {
                                    topic.thumbnails.push(tweet.entities.media[0].media_url);
                                }
                            }
                            tweet.OfficialTimestamp = twitterStringToDate(tweet.created_at);
                            if (tweet.OfficialTimestamp > topic.latestDate) topic.latestDate = tweet.OfficialTimestamp;
                            tweet.searchSource = "twitter";
                            tweet.searchQuery = topic.query;
                            this.searchResults.push(tweet);
                        }
                        timeline.hose.renderItems();
                        console.log("got twitter");
                    }
                });
            }
            if (topic.sources.contains("bing")) {
                serviceQueue++;
                $.ajax({
                    context: ns,
                    type: "GET",
                    url: "BingSearchProxy.aspx?query=" + topic.query + "&latitude=" + ns.latitude + "&longitude=" + ns.longitude,
                    headers: {
                
                    },
                    error: function(err) {
                        serviceQueue--;
                        timeline.hose.renderItems();
                        console.log("bing error " + err.statusText);
                    },
                    success: function(results) {
                        serviceQueue--;
                        var Feed = JSON.parse(results);
                        Feed[0].source = "bing";
                        Feed[0].OfficialTimestamp = new Date();
                        topic.bingCount = Feed[0].Image.length + Feed[0].News.length;
                        this.searchResults = this.searchResults || [];
                        topic.thumbnails = topic.thumbnails || [];
                        topic.latestDate = topic.latestDate || new Date(0);
                        if (Feed[0].Image.length > 0) {
                            topic.thumbnails.push(Feed[0].Image[0].MediaUrl);
                            var ImageAnswer = { images: [] };
                            ImageAnswer.searchSource = "bingimages";
                            ImageAnswer.OfficialTimestamp = new Date();
                            ImageAnswer.searchQuery = topic.query;
                            for (var z = 0; z < Feed[0].Image.length; z++) {
                                ImageAnswer.images.push(Feed[0].Image[z]);
                            }
                            this.searchResults.push(ImageAnswer);
                        }
                        for (var i = 0; i < Feed[0].News.length; i++) {
                            var NewsItem = Feed[0].News[i];
                            var dStart = Feed[0].News[i].Date.indexOf("(") + 1;
                            var dEnd = Feed[0].News[i].Date.indexOf(")");

                            NewsItem.OfficialTimestamp = new Date(parseInt(Feed[0].News[i].Date.substring(dStart, dEnd)));
                            if (NewsItem.OfficialTimestamp > topic.latestDate) topic.latestDate = NewsItem.OfficialTimestamp;
                            NewsItem.searchSource = "bingnews";
                            NewsItem.searchQuery = topic.query;
                            this.searchResults.push(NewsItem);
                        }

                        timeline.hose.renderItems();
                        console.log("got bing");
                    }
                });
            }
        }

        function GetMap(myLat, myLong) {
            console.log(myLat + " / " + myLong);
        }

        // On page init, fetch the data and set up event handlers
        function init() {

            loadFeed();
        }

        // *************************************************************************** //
        //public variables
        ns.currentDay = new Date();
        ns.hose = null;
        ns.latitude = 47.620098;
        ns.longitude = -122.1408;
        ns.searchResults = [];
        ns.timeSlice = "";

        var hrs = ns.currentDay.getHours();
        if (hrs < 24) ns.timeSlice = "evening";
        if (hrs < 16) ns.timeSlice = "afternoon";
        if (hrs < 11) ns.timeSlice = "morning";

        //public methods
        ns.on_map = function(position) {
            ns.latitude = position.coords.latitude;
            ns.longitude = position.coords.longitude;
            GetMap(ns.latitude, ns.longitude);
        }

        //initialization
        init();

    })(springboard);
});