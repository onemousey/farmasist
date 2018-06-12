(function () {

    'use strict';

    angular
       .module('farmasist')
       .controller('ModulFarmacistController', ModulFarmacistController);

    ModulFarmacistController.$inject = ['ModulFarmacistService'];

    function ModulFarmacistController(ModulFarmacistService) {

        //salveaza scopul functiei curente
        var viewModel = this;

        viewModel.cereriMedicamenteAcceptate = [];
        viewModel.loadData = loadData;

        function loadData() {
            ModulFarmacistService.getCereriMedAcceptate()
            .then(
                 function (data) {
                     if (data.IsError) {
                         viewModel.cereriMedicamenteAcceptate.length = 0;
                     }
                     else {
                         viewModel.cereriMedicamenteAcceptate = angular.copy(data.CereriMedicamenteAcceptate);
                     }
                 },
                function (e) {
                    viewModel.cereriMedicamenteAcceptate.length = 0;
                }
            );
        }
    }

})();