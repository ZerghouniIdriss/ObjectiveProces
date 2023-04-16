okrsapp.service('activitiesservice', ['$http', 'localdata', function($http, localdata) {
    this.getActivities = function() {
        this.addToken();
        var promise = $http.post(this.Url('/api/Activity/All'));
        return promise;
    }
    this.addToken = function() {
        var token = 'Bearer ' + localdata.getauthenticationID().aID;
        $http.defaults.headers.common.Authorization = token;
    }
    this.Url = function(url) {
        var returl = localdata.getUriBase() + url;
        return returl;
    }
}]);
