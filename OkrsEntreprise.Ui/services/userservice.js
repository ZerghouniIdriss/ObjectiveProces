okrsapp.service('userservice', ['$http', 'localdata', function ($http, localdata) {


    this.addToken = function () {
        var token = 'Bearer ' + localdata.getauthenticationID().aID;
        $http.defaults.headers.common.Authorization = token;
    }
    this.Url = function (url) {
        var returl = localdata.getUriBase() + url;
        return returl;
    }

    this.listUser = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Account/UserList'), formdata);
        return promise;

    }

    this.deleteUser = function (id) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Account/Delete'), id);
        return promise;

    }

    this.retrievUser = function (id) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Account/Retrieve'), id);
        return promise;

    }


    this.searchteam = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Team/Search'), formdata);
        return promise;

    }


    this.assignTeams = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Account/AssignTeam'), formdata);
        return promise;

    }

    this.deleteTeam = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Account/DeleteTeam'), formdata);
        return promise;

    }

    this.editFirstName = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Account/EditFirstName'), formdata);
        return promise;

    }

    this.editLastName = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Account/EditLastName'), formdata);
        return promise;

    }

    this.editEmail = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Account/EditEmail'), formdata);
        return promise;

    }

    this.getOverallProgress = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Account/GetOverallProgress'), formdata);
        return promise;

    }

    this.getAligned = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Account/GetAligned'), formdata);
        return promise;

    }


    this.uploadImage = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Account/UploadAvatar'), formdata, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })

        return promise;
    }


    this.getRoles = function () {

        this.addToken();
        var promise = $http.post(this.Url('/api/Account/RoleList'));
        return promise;

    }


    this.assignRole = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Account/AssignRole'), formdata);
        return promise;

    }

    this.inviteOthers = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Account/InviteOthers'), formdata);
        return promise;

    }

    this.deleteRole = function (formdata) {

        this.addToken();
        return $http.post(this.Url('/api/Account/DeleteRole'), formdata);

    }
    this.editManager = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Account/EditManager'), formdata);
        return promise;

    }

    this.GetMostRecognized = function () {
        this.addToken();
        var promise = $http.post(this.Url('/api/Account/GetMostRecognized'));
        return promise;
    }

}]);