<template>
  <div id="app" class="container">
    <h3>Статистика текущего пользователя</h3>
    <StatisticsChart backendUrl="http://devsharepoint.csu.ru/_vti_bin/StatisticsServices.svc/GetUserStatistics/"/>
  </div>
</template>

<script>

import StatisticsChart from './components/StatisticsChart';

export default {
  name: 'app',
  components: {
	  StatisticsChart
  }
}
</script>

<style>
  @import "../node_modules/bootstrap/dist/css/bootstrap.min.css";
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
