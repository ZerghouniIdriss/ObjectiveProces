okrsapp.service('goalstatusservice', ['$http', 'localdata', function ($http, localdata) {



    this.createStatus = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/GoalStatus/Create'), formdata);
        return promise;

    }
    this.retrieveStatus = function (id) {

        this.addToken();
        var promise = $http.post(this.Url('/api/GoalStatus/Retrieve'), id);
        return promise;

    }

    this.updateStatus = function (id, title) {

        this.addToken();
        var promise = $http.post(this.Url('/api/GoalStatus/UpdateGoalStatus'), { "id": id, "title": title });
        return promise;

    }

  
    this.listGoalStatus = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/GoalStatus/Search'), formdata);
        return promise;

    }

   
    this.deleteStatus = function (id) {

        this.addToken();
        var promise = $http.post(this.Url('/api/GoalStatus/Delete'), id);
        return promise;

    }


    this.addToken = function () {
        var token = 'Bearer ' + localdata.getauthenticationID().aID;
        $http.defaults.headers.common.Authorization = token;
    }
    this.Url = function (url) {
        var returl = localdata.getUriBase() + url;
        return returl;
    }
}]);