(function () {
    'use strict';

    angular
        .module('app')
        .controller('HomeController', HomeController);

    HomeController.$inject = ['$http','$interval'];

    function HomeController($http, $interval) {
        var vm = this;
        var REFRESH_TIME = 5;

        vm.remainingTimeBeforeRefresh = REFRESH_TIME;

        vm.monitoredProcesses = [];

        vm.rowClass = function (process)
        {
            if (process.State === 0) {
                return "success";
            }
            return "warning";
        }

        refresh();

        function refresh() {
            $interval(function () {
                //each second, we update the remaining time before the call to the server
                vm.remainingTimeBeforeRefresh -= 1;

                //if total time elapsed, we make request to server for status of monitored processes
                //and we re-initialize the remaining time
                if (vm.remainingTimeBeforeRefresh === 0) {
                    $http.get('/Home/GetProcessStatus').then(onRefreshSuccess, onRefreshError);
                }
            }, 1000);
        }

        function onRefreshSuccess(response) {
            vm.remainingTimeBeforeRefresh = REFRESH_TIME;
            vm.monitoredProcesses = response.data;
        }

        function onRefreshError() {
            vm.remainingTimeBeforeRefresh = REFRESH_TIME;
            alert("Error retrieving data.");
        }
    }
})();
