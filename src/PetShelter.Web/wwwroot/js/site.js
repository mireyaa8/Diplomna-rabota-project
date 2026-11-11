// Общ JavaScript за всички страници.

(function () {
    'use strict';

    // ---------- Анимация при превъртане ----------
    // Елементите с клас .reveal се появяват плавно, когато влязат в екрана.
    const revealElements = document.querySelectorAll('.reveal');

    if ('IntersectionObserver' in window) {
        const observer = new IntersectionObserver((entries) => {
            entries.forEach((entry) => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('is-visible');
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: 0.15 });

        revealElements.forEach((el) => observer.observe(el));
    } else {
        revealElements.forEach((el) => el.classList.add('is-visible'));
    }

    // ---------- Съгласие за бисквитки ----------
    const consentButton = document.querySelector('#cookieConsent button[data-cookie-string]');

    if (consentButton) {
        consentButton.addEventListener('click', () => {
            document.cookie = consentButton.dataset.cookieString;
            document.getElementById('cookieConsent').remove();
        });
    }
})();
