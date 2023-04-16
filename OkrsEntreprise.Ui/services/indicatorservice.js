okrsapp.service('indicatorservice', ['$http', 'localdata', function ($http, localdata) {


    this.create = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Indicator/Create'), formdata);
        return promise;

    }

    this.list = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Indicator/List'), formdata);
        return promise;

    }


    this.retrieve = function (id) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Indicator/Retrieve'), id);
        return promise;

    }


    this.delete = function (id) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Indicator/Delete'), id);
        return promise;

    }

    this.update = function (newValue) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Indicator/Update'), { "Id": newValue.Id, "Title": newValue.Title, "Type":newValue.Type });
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