okrsapp.service('dashboardservice', ['$http', 'localdata', function($http, localdata) {

    this.GetOverallProgress = function() {
        this.addToken();
        var promise = $http.get(this.Url('/api/Dashboard/Overall'));
        return promise;
    }

    //this.GetOrksByProgress = function() {
    //    this.addToken();
    //    var promise = $http.get(this.Url('/api/Dashboard/ByProgress'));
    //    return promise;
    //}

    this.GetMyprogress = function () {
        this.addToken();
        var promise = $http.get(this.Url('/api/Dashboard/MyProgress'));
        return promise;
    }

    this.GetOrks = function(filter) {
        this.addToken();
        var promise = $http.get(this.Url('/api/Dashboard/Get?filter=' + filter));
        return promise;
    }

    this.getActivities = function() {
        this.addToken();
        var promise = $http.post(this.Url('/api/Activity/All'));
        return promise;
    }

    this.getRecentActivities = function () {
        this.addToken();
        var promise = $http.post(this.Url('/api/Dashboard/RecentActivities'));
        return promise;
    }

    this.addToken = function () {
        var token = 'Bearer ' + localdata.getauthenticationID().aID;
        $http.defaults.headers.common.Authorization = token;
    }

    this.Url = function(url) {
        var returl = localdata.getUriBase() + url;
        return returl;
    }
}]);
