okrsapp.controller('teamdashboard', ['$scope', '$location', 'localdata', 'dashboardservice',
    function($scope, $location, localdata, dashboardservice) {

        $scope.RecentActivities = [];
        $scope.OnTrackObjectives = [];
        $scope.AtRiskObjectives = [];
        $scope.DelayedObjectives = [];
        $scope.OverallProgress = 0;

        $scope.Init = function() {
            $(".scrollerPanel").niceScroll({
                cursorcolor: "#cfcfcf",
                cursorwidth: "8"
            });

            $scope.GetRecentActivities();
            $scope.DrawByProgressChart();
            $scope.GetOnTrackObjectives();
            $scope.GetRiskObjectives();
            $scope.GetDelayedObjectives();
            $scope.GetOverallProgress();
            $scope.GetOrksByProgress();
        }

        $scope.DrawByProgressChart = function() {
            Morris.Donut({
                element: 'byProgressChart',
                data: [
                    { label: "0-20%", value: 40, color: "#B53D3F" },
                    { label: "20-40%", value: 25, color: "#D85D5F" },
                    { label: "40-60%", value: 10, color: "#F3A538" },
                    { label: "60-80%", value: 15, color: "#FEDD50" },
                    { label: "80-100%", value: 10, color: "#B7E554" },
                ]
            });
        }

        $scope.DrawOverallChart = function() {
            Morris.Donut({
                element: 'overallChart',
                data: [
                    // { label: "", value: $scope.OverallProgress, color: "#f2a200" },
                    // { label: "", value: 100 - $scope.OverallProgress, color: '#e0e0e0' },

                    { label: "", value: 80, color: '#e0e0e0' },
                    { label: "", value: 20, color: "#f2a200" },
                ]
            });
        }

        $scope.GetOverallProgress = function() {
            $scope.loadingImage = true;
            var promise = dashboardservice.GetOverallProgress();
            promise
                .success(function(data) {
                    $scope.loadingImage = false;
                    $scope.OverallProgress = data;
                    $scope.DrawOverallChart();
                })
                .error(function(data, status, headers, config) {
                    if (status = 400) {
                        $scope.loadingImage = false;
                        $scope.ErrorMessage = $scope.parseErrors(data);
                    }
                });
        }

        $scope.GetOrksByProgress = function() {
            $scope.loadingImage = true;
            var promise = dashboardservice.GetOrksByProgress();
            promise
                .success(function(data) {
                    console.log(data);
                    $scope.loadingImage = false;
                    $scope.OverallProgress = data;
                    //   $scope.DrawOverallChart();
                })
                .error(function(data, status, headers, config) {
                    if (status = 400) {
                        $scope.loadingImage = false;
                        $scope.ErrorMessage = $scope.parseErrors(data);
                    }
                });
        }

        $scope.GetOnTrackObjectives = function() {
            $scope.loadingImage = true;
            var promise = dashboardservice.GetOrks('ontrack');
            promise
                .success(function(data) {
                    $scope.loadingImage = false;
                    $scope.OnTrackObjectives = data;
                })
                .error(function(data, status, headers, config) {
                    if (status = 400) {
                        $scope.loadingImage = false;
                        $scope.ErrorMessage = $scope.parseErrors(data);
                    }
                });
        };
        $scope.GetRiskObjectives = function() {
            $scope.loadingImage = true;
            var promise = dashboardservice.GetOrks('atrisk');
            promise
                .success(function(data) {
                    $scope.loadingImage = false;
                    $scope.AtRiskObjectives = data;
                })
                .error(function(data, status, headers, config) {
                    if (status = 400) {
                        $scope.loadingImage = false;
                        $scope.ErrorMessage = $scope.parseErrors(data);
                    }
                });
        };
        $scope.GetDelayedObjectives = function() {
            $scope.loadingImage = true;
            var promise = dashboardservice.GetOrks('delayed');
            promise
                .success(function(data) {
                    $scope.loadingImage = false;
                    $scope.DelayedObjectives = data;
                })
                .error(function(data, status, headers, config) {
                    if (status = 400) {
                        $scope.loadingImage = false;
                        $scope.ErrorMessage = $scope.parseErrors(data);
                    }
                });
        };

         $scope.GetRecentActivities = function() {
            $scope.loadingImage = true;
            var promise = dashboardservice.getActivities();
            promise
                .success(function(data) {
                    $scope.loadingImage = false;
                    $scope.RecentActivities = data;
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
    }
]);
