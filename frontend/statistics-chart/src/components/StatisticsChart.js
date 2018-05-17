import {Line} from 'vue-chartjs';
import axios from 'axios';
import jsonpAdapter from 'axios-jsonp';

export default {
	extends: Line,
	props: {
		backendUrl: {
			type: String
		},
		height: {
			type: Number,
			defaultValue: 200
		}
	},
	data: function () {
		return {
			labels: [],
			datasets: [],
			userId: -1
		}
	},
	mounted () {
		this.userId = _spPageContextInfo.userId;

		if(this.userId === null || this.userId < 0) {
			console.log('could not get userId');
		} else {
			this.loadStatistics()
				.then(() => {
					debugger;
					this.renderChart({
						labels: this.labels,
						datasets: [
							{
								label: 'Статистика для пользователя с id = ' + this.userId,
								backgroundColor: '#f87979',
								data: this.datasets.data
							}
						]
					})
			})
		}


	},
	methods: {

		async loadStatistics () {
			await axios.get(this.backendUrl + this.userId)
			.then((response) => {
				debugger;

				this.datasets.data = response.data.map((x) => {
					return x.Value;
				});

				this.labels = response.data.map((x) => {
					return x.Date;
				});



				console.log(this.dataset);
			}).catch((response) => {
				console.log(response);
			});
		}
	}
}