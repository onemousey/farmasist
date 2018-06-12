(function () {

    'use strict';

    angular
       .module('farmasist')
       .factory('ModulFarmacistService', ModulFarmacistService);

    ModulFarmacistService.$inject = ['$http', '$q'];

    function ModulFarmacistService($http, $q) {

        var service = {
            getCereriMedAcceptate: getCereriMedAcceptate
        };

        return service;

        function getCereriMedAcceptate() {
            var def = $q.defer();

            $http.get('/Farmacie/GetMedicamente')
                .success(function (data) {
                    def.resolve(data);
                })
                .error(function (e) {
                    def.reject("Failed to get data ! Info : " + e);
                });
            return def.promise;

        }

    }

})();