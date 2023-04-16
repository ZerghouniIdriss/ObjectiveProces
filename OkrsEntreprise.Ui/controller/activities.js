okrsapp.controller('activities', ['$scope', '$location', 'localdata', 'activitiesservice', '$anchorScroll',
function($scope, $location, localdata, activitiesservice, $anchorScroll) {

        $scope.RecentActivities = [];
        $scope.OnTrackObjectives = [];
        $scope.AtRiskObjectives = [];
        $scope.DelayedObjectives = [];
        $scope.OverallProgress = 0;
        $scope.ByProgresses = [];

        $scope.Init = function() {
            $(".scrollerPanel").niceScroll({
                cursorcolor: "#cfcfcf",
                cursorwidth: "8"
            });
            $scope.GetRecentActivities();
        }

        $scope.GetRecentActivities = function() {
            $scope.loadingImage = true;
            var promise = activitiesservice.getActivities();
            promise
                .success(function(data) {
                    $scope.loadingImage = false;
                    $scope.RecentActivities = data;
                    //console.log(data);
                })
                .error(function(data, status, headers, config) {
                    if (status = 400) {
                        $scope.loadingImage = false;
                        $scope.ErrorMessage = $scope.parseErrors(data);
                    }
                });
        }

        $scope.timeAgo = function(time) {
            return $.timeago(time);
        }
        $scope.Init();


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

                //$location.hash('activities');
                $anchorScroll();
            }
        };

        $scope.DisablePrevPage = function () {
            return $scope.currentPage === 0 ? "disabled" : "";
        };

        $scope.pageCount = function () {
            return Math.ceil($scope.RecentActivities.length / $scope.itemsPerPage) - 1;
        };

        $scope.nextPage = function () {
            if ($scope.currentPage < $scope.pageCount()) {
                $scope.currentPage++;

                //$location.hash('activities');
                $anchorScroll();

            }
        };

        $scope.DisableNextPage = function () {
            return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
        };

        $scope.setPage = function (n) {
            $scope.currentPage = n;

            //$location.hash('activities');
            $anchorScroll();

        };

        $scope.format = 'MM/dd/yyyy';

    }
]);
