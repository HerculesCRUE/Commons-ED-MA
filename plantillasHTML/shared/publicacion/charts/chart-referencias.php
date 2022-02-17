<div class="chart-wrap">
    <canvas id="chart-publicaciones" class="js-chart" width="600" height="400"></canvas>
    <script>
        var ctx = document.getElementById('chart-publicaciones');
        var parent = ctx.parentElement;
        var width = parent.offsetWidth;
        ctx.setAttribute('width', width);
        var myChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: ['2008', '2009', '2010', '2012', '2013', '2014','2008', '2009', '2010', '2012', '2013', '2014'],
                datasets: [{
                        type: 'line',
                        label: 'Impacto',
                        backgroundColor: '#0b7ad0',
                        borderColor: '#0b7ad0',
                        fill: false,
                        data: [1, 11, 3, 4, 12, 4, 1, 4, 12, 9, 7, 12],
                    },{
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
                scales: {
                    y: {
                        beginAtZero: true
                    }
                },
                plugins: {
                    legend: {
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
