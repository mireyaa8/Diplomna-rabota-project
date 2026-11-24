// Каталог: показва само породите на избрания вид.

(function () {
    'use strict';

    const speciesSelect = document.getElementById('SpeciesId');
    const breedSelect = document.getElementById('BreedId');

    if (!speciesSelect || !breedSelect) {
        return;
    }

    function filterBreeds() {
        const speciesId = speciesSelect.value;

        Array.from(breedSelect.options).forEach((option) => {
            if (!option.value) {
                return; // „Всички“ остава винаги видимо
            }

            const matches = !speciesId || option.dataset.species === speciesId;
            option.hidden = !matches;

            // Ако избраната порода не е от този вид, изчистваме избора.
            if (!matches && option.selected) {
                breedSelect.value = '';
            }
        });
    }

    speciesSelect.addEventListener('change', filterBreeds);
    filterBreeds();
})();
