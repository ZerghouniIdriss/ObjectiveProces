okrsapp.controller('closedgoals', ['$scope', '$location', '$routeParams', 'localdata', 'goalservice', function ($scope, $location, $routeParams, localdata, goalservice) {

    $scope.loadingImage = false;
    $scope.ErrorMessage = '';
    $scope.searchformData = {};
    $scope.goalList = [];
    $scope.closedGoals = [];
    $scope.lastgoal = null;
    $scope.defcat = 1;
    $scope.goalstatusList = [];

    $scope.onSuccess = function () {
        $location.path('/closedobjectives');
    }

    $scope.processsearchform = function () {  
        var promise = goalservice.GetClosedGoals($scope.searchformData);
        promise.success(function (data) {  
            $scope.closedGoals = data;
          })
         .error(function (data, status, headers, config) {
             if (status == 400) { 
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
             else if (status == 404) { 
                 $scope.ErrorMessage = 'Requested link not found';
             }
             $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage); 
         });

    }

    $scope.clearall = function () {

        $scope.searchformData.id = '';
        $scope.searchformData.title = '';
        $scope.searchformData.userteam = '';
        $scope.searchformData.category = $scope.goalcatList[0];
        $scope.searchformData.status = $scope.goalstatusList[0];
        $scope.searchformData.privateonly = '';
        $scope.searchformData.alignedonly = '';

        $scope.searchformData.privateonlycheckbox = false;
        $scope.searchformData.alignedonlycheckbox = false;

    }

    $scope.Init = function () {


        $scope.searchformData.id = '';
        $scope.searchformData.title = '';
        $scope.searchformData.userteam = '';
        $scope.searchformData.category = {};
        $scope.searchformData.privateonly = '';
        $scope.searchformData.alignedonly = '';

        $scope.processsearchform();
        $scope.getgoalstatus();
        $scope.getgoalcat();

        if ($routeParams.query != undefined) {
            $scope.searchformData.title = $routeParams.query;
        }
        
         
       
    }

    $scope.getgoalcat = function () {

        $scope.loadingImage = true;
        var promise = goalservice.searchgoalcat();
        promise
          .success(function (data) {

              var allgoal = { "id": "", "title": "All" };
              data.splice(0, 0, allgoal);

              $scope.loadingImage = false;
              $scope.goalcatList = data;

              $scope.searchformData.category = allgoal;

          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
             else if (status == 404) {
                 $scope.ErrorMessage = 'Requested link not found';
             }
             $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)

         });

    }

    $scope.getgoalstatus = function () {

        $scope.loadingImage = true;
        var promise = goalservice.searchgoalstatus();
        promise
          .success(function (data) {

              var allgoal = { "id": "", "title": "All" };
              data.splice(0, 0, allgoal);

              $scope.loadingImage = false;
              $scope.goalstatusList = data;

              $scope.searchformData.status = allgoal;

          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
             else if (status == 404) {
                 $scope.ErrorMessage = 'Requested link not found';
             }
             $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)

         });

    }

    $scope.Init();
    $scope.privateClicked = function () {

        if ($scope.searchformData.privateonlycheckbox) {

            $scope.searchformData.privateonly = true;
        }
        else {
            $scope.searchformData.privateonly = '';
        }
    }

    $scope.alignedClicked = function () {

        if ($scope.searchformData.alignedonlycheckbox) {

            $scope.searchformData.alignedonly = true;
        }
        else {
            $scope.searchformData.alignedonly = '';
        }
    }

    $scope.itemsPerPage = 10;
    $scope.currentPage = 0;

    $scope.range = function () {

        var rangeSize = 5;
        var ps = [];
        var start;
        var end;

        if ($scope.pageCount() > rangeSize) {
            start = $scope.currentPage <= 0 ? 0 : $scope.currentPage - 1;
            end = start + (rangeSize - 1);

            var diff = start + (rangeSize - 1) - $scope.pageCount();

            if (diff > 0) {
                start = start - diff;
                end = end - diff;
            }

        }
        else {
            start = 0;
            end = $scope.pageCount()
        }

        for (var i = start; i <= end; i++) {
            ps.push(i);
        }

        return ps;
    };

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;

           
        }
    };

    $scope.DisablePrevPage = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.pageCount = function () {
        return Math.ceil($scope.closedGoals.length / $scope.itemsPerPage) - 1;
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount()) {
            $scope.currentPage++;
 

        }
    };

    $scope.DisableNextPage = function () {
        return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
    };

    $scope.setPage = function (n) {
        $scope.currentPage = n; 

    };

    $scope.format = 'MM/dd/yyyy';

}
]);
