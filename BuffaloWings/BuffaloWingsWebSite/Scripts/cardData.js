var cardData = [
    {
        momentName: "French Fries",
        startTime: 0,
        endTime: 10,
        topics:
            [
	        {
	            cardTitle: "Trending Articles on Facebook",
	            content: "",
	            template: "api",
	            endpoint: "http://blizzard.cloudapp.net/actions.pl?action=fbnews",
	            actions: []
	        },
	        {
	            cardTitle: "BingNow Trending News",
	            content: "",
	            template: "api",
	            endpoint: "http://blizzard.cloudapp.net/actions.pl?action=bingnow",
	            actions: []
	        },
	        {
	            cardTitle: "Popular Youtube Videos under 90 seconds",
	            content: "",
	            template: "api",
	            endpoint: "http://blizzard.cloudapp.net/actions.pl?action=youtube",
	            actions: []
	        },
	        {
	            cardTitle: "Trending Twitter HashTags",
	            content: "",
	            template: "api",
	            endpoint: "http://blizzard.cloudapp.net/api/twitter",
	            actions: []
	        },
            {
	            cardTitle: "Popular Videos on Vine",
	            content: "",
	            template: "api",
	            endpoint: "http://blizzard.cloudapp.net/api/vine",
	            actions: []
            },
            {
                cardTitle: "Trending Instagram Hashtags",
                content: "",
                template: "api",
                endpoint: "http://blizzard.cloudapp.net/api/instagram",
                actions: []
            },
            {
                cardTitle: "Popular Photos on imgur",
                content: "",
                template: "api",
                endpoint: "http://blizzard.cloudapp.net/api/imgur",
                actions: []
            }
        ]
    }
];

function logData(vote, card, image) {
    var undo = false;
    var currImage = image.src;
    if (vote == 'up') {
        if (currImage.indexOf("voteupgreen.png") > -1) {
            image.src = "\Images/voteup.png";
            undo = true;
        } else {
            image.src = "\Images/voteupgreen.png";
        }
    } else if (vote == 'down') {
        if (currImage.indexOf("votedowngreen.png") > -1) {
            image.src = "\Images/votedown.png";
            undo = true;
        } else {
            image.src = "\Images/votedowngreen.png";
        }
    }

    console.log("Log feedback data");
    var title = card.getElementsByClassName("context");
    var value = title[0].innerHTML;
    $.ajax({

        type: "POST",
        url: "/api/Endpoints/Post",
        context: card,
        data: {
            "title": value,
            "vote": vote,
            "undo": undo
        },
        headers: {
            "api": "http://blizzard.cloudapp.net/api/logData",
            "Accept": "application/json"
        },

        error: function (err) {
            console.log("error" + err);

        },
        success: function (results) {
            console.log("results:" + results);

        }
    });
}