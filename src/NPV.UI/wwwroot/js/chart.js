//this file requires the tag <script src="https://cdn.jsdelivr.net/npm/apexcharts"></script> in index.html

window.blazorApexCharts = window.blazorApexCharts || {};

export function createOrUpdateChart(elementId, series, categories, options) {
    const chartElement = document.getElementById(elementId);

    if (!chartElement) {
        console.error(`Chart element with ID '${elementId}' not found.`);
        return;
    }

    options.series = series;
    options.xaxis = options.xaxis || {};
    options.xaxis.categories = categories;

    const currencyFormatter = function (val) {
        return val.toLocaleString('en-US', { style: 'currency', currency: 'USD' });
    };

    if (options.dataLabels) {
        options.dataLabels.formatter = currencyFormatter;
    }
    if (options.yaxis && options.yaxis.labels) {
        options.yaxis.labels.formatter = currencyFormatter;
    }
    if (options.tooltip && options.tooltip.y) {
        options.tooltip.y.formatter = currencyFormatter;
    }

    if (window.blazorApexCharts[elementId]) {
        console.log(`Updating chart with ID: ${elementId}`);
        window.blazorApexCharts[elementId].updateOptions(options);
    } else {
        console.log(`Creating new chart with ID: ${elementId}`);
        const chart = new ApexCharts(chartElement, options);
        chart.render();
        window.blazorApexCharts[elementId] = chart;
    }
}

export function destroyChart(elementId) {
    if (window.blazorApexCharts[elementId]) {
        console.log(`Destroying chart with ID: ${elementId}`);
        window.blazorApexCharts[elementId].destroy();
        delete window.blazorApexCharts[elementId];
    }
}