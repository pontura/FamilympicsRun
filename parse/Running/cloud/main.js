
Parse.Cloud.define("hello", function(request, response) {
  response.success("Hello world!" + request.params.user);
});

Parse.Cloud.define("challengeUpdate", function(request, response) {
  var query = new Parse.Query(Parse.Installation);
    query.equalTo('user', 'id_' + facebookFriendName);
    Parse.Push.send({
        where: query,
        data: {
            alert: "Challenge_Test"
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



