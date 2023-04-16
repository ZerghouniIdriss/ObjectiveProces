okrsapp.service('recognitionservice', ['$http', 'localdata', function ($http, localdata) {

    // JUST FOR Referance

    //this.createRecognition = function (formdata) {

    //    this.addToken();
    //    var promise = $http.post(this.Url('/api/Recognition/Create'), formdata);
    //    return promise;

    //}

    //this.listRecognition = function (formdata) {

    //    this.addToken();
    //    var promise = $http.post(this.Url('/api/Recognition/RecognitionList'), formdata);
    //    return promise;

    //}


    //this.retrievRecognition = function (id) {

    //    this.addToken();
    //    var promise = $http.post(this.Url('/api/Recognition/Retrieve'), id);
    //    return promise;

    //}


    //this.deleteRecognition = function (id) {

    //    this.addToken();
    //    var promise = $http.post(this.Url('/api/Recognition/Delete'), id);
    //    return promise;

    //}

    //this.updateRecognition = function (id, name) {

    //    this.addToken();
    //    var promise = $http.post(this.Url('/api/Recognition/UpdateTenat'), { "id": id, "name": name });
    //    return promise;

    //}

    this.createRecognition = function (formdata) {
        this.addToken();
        var promise = $http.post(this.Url('/api/Recognition/Create'), formdata);
        return promise;
    }

    this.GetAllRecognitions = function () {
        this.addToken();
        var promise = $http.post(this.Url('/api/Recognition/All'));
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