okrsapp.controller('employeesessiondetail', ['$scope', '$location', '$routeParams', 'localdata', 'dataservice', function ($scope, $location, $routeParams, localdata, dataservice) {

    //$scope.formData = { "Id": 1, "CreatedDate": "2016-01-27T15:28:34.047", "Attendee": { "id": 2, "name": "user2", "isteam": false }, "Animator": { "id": 1, "name": "user1", "isteam": false }, "Goals": [{ "id": 1, "title": "Goal number 1, By user1", "assignees": [{ "id": 1, "name": "user1", "isteam": false }], "category": { "id": 1, "title": "Personal" }, "isprivate": false, "description": null, "duedate": null, "status": { "id": 2, "title": "In Progress" }, "keyresults": [{ "id": 1, "goalid": 0, "title": "Key result 1 of goals 1", "status": null }, { "id": 2, "goalid": 0, "title": "Key result 2 of goals 1", "status": null }], "createddate": "2016-01-27T15:28:33.497", "comments": [], "parent": null, "children": [] }, { "id": 2, "title": "Goal number 2, By user2", "assignees": [{ "id": 2, "name": "user2", "isteam": false }], "category": { "id": 1, "title": "Personal" }, "isprivate": false, "description": null, "duedate": null, "status": { "id": 3, "title": "Achieved" }, "keyresults": [{ "id": 3, "goalid": 0, "title": "Key result 1 of goals 2", "status": null }, { "id": 4, "goalid": 0, "title": "Key result 2 of goals 2", "status": null }], "createddate": "2016-01-27T15:28:33.497", "comments": [], "parent": null, "children": [] }, { "id": 3, "title": "Goal number 3, by both users", "assignees": [{ "id": 1, "name": "user1", "isteam": false }, { "id": 2, "name": "user2", "isteam": false }], "category": { "id": 1, "title": "Personal" }, "isprivate": false, "description": null, "duedate": null, "status": { "id": 1, "title": "New" }, "keyresults": [{ "id": 5, "goalid": 0, "title": "Key result 1 of goals 3", "status": null }, { "id": 6, "goalid": 0, "title": "Key result 2 of goals 3", "status": null }, { "id": 7, "goalid": 0, "title": "Key result 3 of goals 3", "status": null }, { "id": 8, "goalid": 0, "title": "Key result 4 of goals 3", "status": null }], "createddate": "2016-01-27T15:28:33.497", "comments": [], "parent": null, "children": [] }], "SessionStatus": { "title" : "Some Titel" }, "Note": "SOmesd sdfksldf lsdfksldmfsld" };
    $scope.formData = {};

    $scope.Init = function () {
        //alert($routeParams.id);
        $scope.getsession($routeParams.id);
    }

    $scope.getsession = function (id) {
        $scope.loadingImage = true;
        var promise = dataservice.getEmpEvalSession(id);
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.formData = data;

          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
             else
                 if (status == 404) {

                     $scope.loadingImage = false;
                     $scope.ErrorMessage = 'Requested link not found';
                 }
             $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
         });

    }

    $scope.parseErrors = function (response) {
        var errors = [];
        for (var key in response.ModelState) {
            for (var i = 0; i < response.ModelState[key].length; i++) {
                errors.push(response.ModelState[key][i]);
            }
        }
        return errors.toString();
    }

    $scope.Init();
}]);