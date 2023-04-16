okrsapp.service('tenantservice', ['$http', 'localdata', function ($http, localdata) {


    this.createTenant = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Tenant/Create'), formdata);
        return promise;

    }

    this.listTenant = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Tenant/TenantList'), formdata);
        return promise;

    }


    this.retrievTenant = function (id) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Tenant/Retrieve'), id);
        return promise;

    }


    this.deleteTenant = function (id) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Tenant/Delete'), id);
        return promise;

    }

    this.updateTenant = function (id, name) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Tenant/UpdateTenat'), { "id": id, "name": name });
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