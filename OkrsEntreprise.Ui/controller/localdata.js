okrsapp.factory('localdata', function ($rootScope) {

    var authenticationID;
    var userData;

    var init = function() {
        var _userId = localStorage.getItem('aid');
        if (_userId != null) {
            authenticationID = _userId;
        }

        var _userData = localStorage.getItem('userData');
        if (_userData != null) {
            
            userData = JSON.parse(_userData);
        }

    };

    init();

    return {
        setauthenticationID: function(aid) {

            authenticationID = aid;
            localStorage.setItem('aid', authenticationID);
        },

        getauthenticationID: function() {
            return {
                aID: authenticationID,
            }
        },

        setuserData: function(un) {

            userData = un;
            localStorage.setItem('userData', JSON.stringify(un));
            $rootScope.$broadcast('userDataChanged');
        },

        getuserData: function() {
            return {
                un: userData,
            }
        },

        Logout: function() {
            authenticationID = null;
            localStorage.clear();
        },

        getUriBase: function() {
                 return 'http://localhost:65365';
                  //  return 'http://45.34.14.176/Api';

        },

        parseErrors: function (response) {
            var errors = [];
            for (var key in response.ModelState) {
                for (var i = 0; i < response.ModelState[key].length; i++) {
                    errors.push(response.ModelState[key][i]);
                }
            }
            return errors.toString();
        }

    };
});
