
export function initializeTooltips(selector = '[data-bs-toggle="tooltip"]') {

    var tooltipTriggerList = [].slice.call(document.querySelectorAll(selector))

    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    })
}

export function disposeTooltips(selector = '[data-bs-toggle="tooltip"]') {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll(selector))
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        var tooltipInstance = bootstrap.Tooltip.getInstance(tooltipTriggerEl);
        if (tooltipInstance) {
            tooltipInstance.dispose();
        }
    });
}