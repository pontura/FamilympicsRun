
Parse.Cloud.define("hello", function(request, response) {
  response.success("Hello world!" + request.params.user);
});

Parse.Cloud.define("challengeUpdate", function(request, response) {
  var query = new Parse.Query(Parse.Installation);
    query.equalTo('facebookID', 'id_' + request.params.facebookFriendId);
    Parse.Push.send({
        where: query,
        data: {
            alert: request.params.username + " sent you a new challenge in level " + request.params.levelId
        }
    }, {
    success: function() {
        console.log("success: Parse.Push.send envio challenge");
    },
    error: function(e) {
        console.log("error: Parse.Push.send code: " + e.code + " msg: " + e.message);
    }
    });
});

Parse.Cloud.define("challengeBeatYou", function(request, response) {
  var query = new Parse.Query(Parse.Installation);
    query.equalTo('facebookID', 'id_' + request.params.facebookFriendId);
    Parse.Push.send({
        where: query,
        data: {
            alert: request.params.username + " accepted your challenge and beat you!"
        }
    }, {
    success: function() {
        console.log("success: Parse.Push.send envio challenge");
    },
    error: function(e) {
        console.log("error: Parse.Push.send code: " + e.code + " msg: " + e.message);
    }
    });
});

Parse.Cloud.define("challengeLost", function(request, response) {
  var query = new Parse.Query(Parse.Installation);
    query.equalTo('facebookID', 'id_' + request.params.facebookFriendId);
    Parse.Push.send({
        where: query,
        data: {
            alert: request.params.username + " accepted your challenge and lost!"
        }
    }, {
    success: function() {
        console.log("success: Parse.Push.send envio challenge");
    },
    error: function(e) {
        console.log("error: Parse.Push.send code: " + e.code + " msg: " + e.message);
    }
    });
});

Parse.Cloud.define("challengeRemind", function(request, response) {
  var query = new Parse.Query(Parse.Installation);
    query.equalTo('facebookID', 'id_' + request.params.facebookFriendId);
    Parse.Push.send({
        where: query,
        data: {
            alert: "Challenge reminder from " + request.params.username
        }
    }, {
    success: function() {
        console.log("success: Parse.Push.send envio challenge");
    },
    error: function(e) {
        console.log("error: Parse.Push.send code: " + e.code + " msg: " + e.message);
    }
    });
});

Parse.Cloud.define("sendNotificationTo", function(request, response) {
  var query = new Parse.Query(Parse.Installation);
    query.equalTo('facebookID', 'id_' + request.params.facebookFriendId);
    Parse.Push.send({
        where: query,
        data: {
            alert: request.params.username + " is requesting energy."
        }
    }, {
    success: function() {
        console.log("success: Parse.Push.send envio challenge");
    },
    error: function(e) {
        console.log("error: Parse.Push.send code: " + e.code + " msg: " + e.message);
    }
    });
});

Parse.Cloud.define("sentYouEnergy", function(request, response) {
  var query = new Parse.Query(Parse.Installation);
    query.equalTo('facebookID', 'id_' + request.params.facebookFriendId);
    Parse.Push.send({
        where: query,
        data: {
            alert: request.params.username + " sent you energy! Get back in the game."
        }
    }, {
    success: function() {
        console.log("success: Parse.Push.send envio challenge");
    },
    error: function(e) {
        console.log("error: Parse.Push.send code: " + e.code + " msg: " + e.message);
    }
    });
});

// Parse.Cloud.afterSave( "Challenges", function(request) {

	  // var facebookID = request.object.get("facebookID");
	  
	  // var pushQuery = new Parse.Query(Parse.Installation);
	  // pushQuery.equalTo("facebookID",facebookID);

	  // //Send Push message
	  // Parse.Push.send({
					  // where: pushQuery,
					  // data: {
					  // alert: "New Chalenge!",
					  // sound: "default"
					  // }
					  // },{
					  // success: function(){
					  // response.success('true');
					  // },
					  // error: function (error) {
					  // response.error(error);
					  // }
	 // });
// });



