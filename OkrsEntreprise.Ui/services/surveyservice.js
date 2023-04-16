okrsapp.service('surveyservice', ['$http', 'localdata', function ($http, localdata) {
   
    this.createsurvey = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Survey/Create'), formdata);
        return promise;
    }

    this.editsurvey = function (formdata) {
        this.addToken();
        var promise = $http.post(this.Url('/api/Survey/Edit'), formdata);
        return promise;
    }

    this.deletesurvey = function (id) {
        this.addToken();
        var promise = $http.post(this.Url('/api/Survey/Delete'), id);
        return promise;
    }

    this.searchsurvey = function (formdata) {
        this.addToken();
        var promise = $http.post(this.Url('/api/Survey/Search'), formdata);
        return promise;
    }

   
    this.retrievsurvey = function (id) {
        this.addToken();
        var promise = $http.post(this.Url('/api/Survey/Retrieve'), id);
        return promise;
    }

    this.editsurveytitle = function (id, code) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Survey/Edit'), { "id": id, "code": code });
        return promise;

    }
 

  
    this.searchsurvey = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Survey/Search'), formdata);
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