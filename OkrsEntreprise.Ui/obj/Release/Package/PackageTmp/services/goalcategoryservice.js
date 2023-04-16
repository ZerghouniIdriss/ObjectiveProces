okrsapp.service('goalcategoryservice', ['$http', 'localdata', function ($http, localdata) {



    this.createCategory = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/GoalCategory/Create'), formdata);
        return promise;

    }
    this.retrieveCategory = function (id) {

        this.addToken();
        var promise = $http.post(this.Url('/api/GoalCategory/Retrieve'), id);
        return promise;

    }

    this.updateCategory = function (id, title) {

        this.addToken();
        var promise = $http.post(this.Url('/api/GoalCategory/UpdateGoalCategory'), { "id": id, "title": title });
        return promise;

    }

  
    this.listGoalCategory = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/GoalCategory/Search'), formdata);
        return promise;

    }

   
    this.deleteCategory = function (id) {

        this.addToken();
        var promise = $http.post(this.Url('/api/GoalCategory/Delete'), id);
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