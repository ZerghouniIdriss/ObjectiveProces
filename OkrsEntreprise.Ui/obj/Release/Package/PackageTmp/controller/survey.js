okrsapp.controller('survey', ['$scope', '$location', '$routeParams', 'localdata', 'surveyservice', function ($scope, $location, $routeParams, localdata, surveyservice) {

    $scope.loadingImage = false;
    $scope.ErrorMessage = '';
    $scope.addformData = {}; 
    $scope.searchformData = {}; 
    $scope.surveyList = [];
    

    $scope.processsearchform = function () {

        $scope.loadingImage = true;
        var promise = surveyservice.searchsurvey($scope.addformData);
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.surveyList = data;
          })
         .error(function (data, status, headers, config) {
             if (status = 400) {
                 $scope.loadingImage = false; 
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
         });

    }

    $scope.addnewForm = function () {

        $scope.loadingImage = true;
        var promise = surveyservice.createsurvey($scope.addformData);
        promise
          .success(function (data) {
              $scope.loadingImage = false;
              alert($scope.addformData.code + ' added to database')
              if ($routeParams.ret != undefined) {

                  var pvalues = $location.path().split('/');
                  var newpath = '';

                  for (var i = 1; i < pvalues.length - 1; i++) {

                      newpath = newpath + '/' + pvalues[i];
                  }

                  $location.path(newpath);
              }
              else {
                  $location.path('/surveys');
              }
          })
         .error(function (data, status, headers, config) {
             if (status = 400) {
                 $scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
         });

    }

    $scope.Init = function () { 
        $scope.processsearchform();
    }

    $scope.Init();

    $scope.editsurvey = function (id) {
        //$scope.selsurvey = angular.copy(survey);
        $location.path('/editsurvey/' + id.toString());
    };
     
    $scope.NavigateToNewSurvey = function () {
        $location.path('/survey');
    }

    $scope.backtosurveys = function () {
        $location.path('/surveys');
    }


    $scope.addnewForm = function () {

        $scope.loadingImage = true;
        var promise = surveyservice.createsurvey($scope.addformData);
        promise.success(function (data) {
              $scope.loadingImage = false;
              alert($scope.addformData.code + ' added to database')
              if ($routeParams.ret != undefined) {

                  var pvalues = $location.path().split('/');
                  var newpath = '';

                  for (var i = 1; i < pvalues.length - 1; i++) {

                      newpath = newpath + '/' + pvalues[i];
                  }

                  $location.path(newpath);
              }
              else {
                  $location.path('/surveys');
              }
          })
         .error(function (data, status, headers, config) {
             if (status = 400) {
                 $scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
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
        return Math.ceil($scope.surveyList.length / $scope.itemsPerPage) - 1;
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

    $scope.sortType = 'title'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order
    $scope.format = 'MM/dd/yyyy';

  
}]);