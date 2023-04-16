okrsapp.controller('dashboard', ['$scope', '$location', 'localdata', 'dashboardservice',
    function($scope, $location, localdata, dashboardservice) {

        $scope.RecentActivities = [];
        $scope.OnTrackObjectives = [];
        $scope.AtRiskObjectives = [];
        $scope.DelayedObjectives = [];
        $scope.OverallProgress = 0;
        $scope.MyProgress = 0;
       // $scope.ByProgresses = [];
        $scope.isCollapsed = false;
        $scope.isCollapsedHorizontal = false;
        $scope.Init = function() {
            $(".scrollerPanel").niceScroll({
                cursorcolor: "#cfcfcf",
                cursorwidth: "8"
            });

            $scope.GetRecentActivities();
           // $scope.DrawByProgressChart();
            $scope.GetOnTrackObjectives();
            $scope.GetRiskObjectives();
            $scope.GetDelayedObjectives();
            $scope.trackScrollerarrow = false;
            $scope.GetOverallProgress();
            // $scope.GetOrksByProgress();
            $scope.GetMyprogress();
        }
        $scope.scrollPanel = function (id)
        {
            //console.log($('#' + id).scrollTop());
            $('#' + id).scrollTop($('#' + id).scrollTop()+30);
        }
        $scope.showArrow = function (element)
        {
            element = true;
        }
        $scope.hidearrow = function (element) {
            element = false;
        }
        
        $scope.DrawByProgressChart = function() {
            Morris.Donut({
                element: 'byProgressChart',
                data: [
                   { label: "", value: $scope.MyProgress, color: "#34c73b" },
                   { label: "", value: 100 - $scope.MyProgress, color: '#e0e0e0' },
                ]
                
            });
        }

        $scope.DrawOverallChart = function() {
            Morris.Donut({
                element: 'overallChart',
                data: [
                    { label: "", value: $scope.OverallProgress, color: "#34c73b" },
                    { label: "", value: 100 - $scope.OverallProgress, color: '#e0e0e0' },
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
                    console.log(data);
                    $scope.DrawOverallChart();
                })
                .error(function(data, status, headers, config) {
                    if (status = 400) {
                        $scope.loadingImage = false;
                        $scope.ErrorMessage = $scope.parseErrors(data);
                    }
                });
        }


        $scope.GetMyprogress = function () {
            $scope.loadingImage = true;
            var promise = dashboardservice.GetMyprogress();
            promise
                .success(function (data) {
                   
                    $scope.loadingImage = false;
                    $scope.MyProgress = data;
                    console.log(data);
                    $scope.DrawByProgressChart();
                })
                .error(function (data, status, headers, config) {
                    if (status = 400) {
                        $scope.loadingImage = false;
                        $scope.ErrorMessage = $scope.parseErrors(data);
                    }
                });
        }

        //$scope.GetOrksByProgress = function() {
        //    $scope.loadingImage = true;
        //    var promise = dashboardservice.GetOrksByProgress();
        //    promise
        //        .success(function(data) {
        //            $.each(data, function(key, value) {
        //                $scope.ByProgresses.push({
        //                    'label': value.Key,
        //                    'value': parseInt(value.Value),
        //                    'color': GetProgressColorByName(value.Key)
        //                });
        //            });
        //            $scope.DrawByProgressChart();
        //            $scope.loadingImage = false;
        //        })
        //        .error(function(data, status, headers, config) {
        //            if (status = 400) {
        //                $scope.loadingImage = false;
        //                $scope.ErrorMessage = $scope.parseErrors(data);
        //            }
        //        });
        //}

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
            var promise = dashboardservice.getRecentActivities();
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

        $scope.parseErrors = function (response) {
            var errors = [];
            for (var key in response.ModelState) {
                for (var i = 0; i < response.ModelState[key].length; i++) {
                    errors.push(response.ModelState[key][i]);
                }
            }
            return errors.toString();
        }


        function GetProgressColorByName(name) {
            switch (name) {
                case '0-20%':
                    return '#B53D3F';
                    break;
                case '20-40%':
                    return '#D85D5F';
                    break;
                case '40-60%':
                    return '#F3A538';
                    break;
                case '60-80%':
                    return '#FEDD50';
                    break;
                case '80-100%':
                    return '#B7E554';
                    break;
            }
        }
        $scope.Init();
    }
]);
