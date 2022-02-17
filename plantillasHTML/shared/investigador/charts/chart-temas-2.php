<div class="chart-wrap">
    <canvas id="chart-temas2" class="js-chart" width="600" height="400"></canvas>
    <script>
        var ctx = document.getElementById('chart-temas2');
        var parent = ctx.parentElement;
        var width = parent.offsetWidth;
        ctx.setAttribute('width', width);
        var myChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: ['2008', '2009', '2010', '2012', '2013', '2014','2008', '2009', '2010', '2012', '2013', '2014'],
                datasets: [{
                        label: 'Publicaciones',
                        data: [12, 19, 3, 5, 2, 3, 1, 14, 14, 10, 2, 6],
                        backgroundColor: [
                            '#6cafe3',
                            '#6cafe3',
                            '#6cafe3',
                            '#6cafe3',
                            '#6cafe3',
                            '#6cafe3',
                            '#6cafe3',
                            '#6cafe3',
                            '#6cafe3',
                            '#6cafe3',
                            '#6cafe3',
                            '#6cafe3',
                        ],
                        borderColor: [
                            '#6cafe3',
                            '#6cafe3',
                            '#6cafe3',
                            '#6cafe3',
                            '#6cafe3',
                            '#6cafe3',
                            '#6cafe3',
                            '#6cafe3',
                            '#6cafe3',
                            '#6cafe3',
                            '#6cafe3',
                            '#6cafe3',
                        ],
                        borderWidth: 1
                    }
                ]
            },
            options: {
                indexAxis: 'y',
                scales: {
                    y: {
                        beginAtZero: true
                    }
                },
                plugins: {
                    legend: {
                        display: false,
                        labels: {
                            usePointStyle: true,
                        },
                        position: 'top',
                        align: 'start',
                    }
                }
            }
        });
    </script>
</div>
