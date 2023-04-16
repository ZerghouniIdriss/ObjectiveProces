okrsapp.service('teamservice', ['$http', 'localdata', function ($http, localdata) {


    this.createTeam = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Team/Create'), formdata);
        return promise;

    }

    this.listTeam = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Team/TeamList'), formdata);
        return promise;

    }

    this.deleteTeam = function (id) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Team/Delete'), id);
        return promise;

    }

    this.retrieveTeam = function (id) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Team/Retrieve'), id);
        return promise;

    }

    this.editNM = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Team/EditName'), formdata);
        return promise;

    }

    this.editDes = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Team/EditDescription'), formdata);
        return promise;

    }


    this.getOverallProgress = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Team/GetOverallProgress'), formdata);
        return promise;

    }

    this.getAligned = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Team/GetAligned'), formdata);
        return promise;

    }

    this.uploadImage = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Team/UploadAvatar'), formdata, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })

        return promise;
    }
 
    this.searchuser = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Account/UserList'), formdata);
        return promise;

    }

    this.deleteUser = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Team/DeleteUser'), formdata);
        return promise;

    }

    this.addUser = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Team/AddUser'), formdata);
        return promise;

    }

    this.userAssigneeList = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Account/AssigneeList'), formdata);
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