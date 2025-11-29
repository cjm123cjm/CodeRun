<template>
  <div class="patr-panel">
    <el-card class="box-card">
      <div slot="header" class="card-title">
        <span>数据概括</span>
      </div>
      <div class="data-list">
        <el-row :gutter="10">
          <el-col :span="4" v-for="item in allDataList">
            <div class="data-item">
              <div class="title">{{ item.statisticsName }}</div>
              <div class="data-panel">
                <div class="data">{{ item.count }}</div>
                <div class="pre">
                  昨日新增：<span class="new">{{ item.preCount }}</span>
                </div>
              </div>
            </div>
          </el-col>
        </el-row>
      </div>
    </el-card>
  </div>
  <div class="patr-panel">
    <el-card class="box-card">
      <div slot="header" class="card-title">
        <span>近一周数据概括</span>
      </div>
      <div class="data-list">
        <el-row :gutter="10">
          <el-col :span="12">
            <div ref="myChartAppWeekDataRef" class="chart"></div>
          </el-col>
          <el-col :span="12">
            <div ref="myChartContentWeekDataRef" class="chart"></div>
          </el-col>
        </el-row>
      </div>
    </el-card>
  </div>
</template>

<script setup>
import * as echarts from 'echarts'
import { ref, getCurrentInstance, shallowRef, nextTick } from 'vue'
const { proxy } = getCurrentInstance()

const api = {
  getAllData: '/Index/GetAllData',
  getAppWeekData: '/Index/GetAppWeekData',
  getContentWeekData: '/Index/GetContentWeekData',
}

const getOption = (myTitle, xAxisData = [], seriesData = []) => {
  return {
    title: {
      text: myTitle,
    },
    tooltip: {
      trigger: 'axis',
      axisPointer: {
        type: 'shadow',
        textStyle: {
          color: 'red',
        },
      },
    },
    legend: {
      top: 0,
      right: 0,
    },
    grid: {
      left: 50,
      right: 0,
    },
    xAxis: {
      axisLine: {
        lineStyle: {
          color: '#90979c',
        },
      },
      data: xAxisData,
      axisLabel: {
        interval: 0,
        rotate: 45,
      },
    },
    yAxis: {
      type: 'value',
    },
    series: seriesData,
  }
}

//查询数据
const allDataList = ref([])
const loadAllData = async () => {
  let result = await proxy.Request({
    url: api.getAllData,
    dataType: 'json',
    method: 'get',
  })
  if (!result) return

  allDataList.value = result.result
}

//查询柱状图数据1
const myChartAppWeekDataRef = ref()
const myChartAppWeekData = shallowRef()
const loadAppWeekData = async () => {
  let result = await proxy.Request({
    url: api.getAppWeekData,
    dataType: 'json',
    method: 'get',
  })
  if (!result) return

  const data = result.result
  const xAxisData = data.dataList
  const seriesData = []
  const colors = ['#1b9cfc', '#67c23a']
  data.itemDataList.forEach((item, index) => {
    seriesData.push({
      name: item.statisticsName,
      type: 'bar',
      data: item.listData,
      itemStyle: {
        color: colors[index],
      },
    })
  })
  myChartAppWeekData.value.setOption(getOption('App下载注册用户统计', xAxisData, seriesData))
}

//查询柱状图数据2
const myChartContentWeekDataRef = ref()
const myChartContentWeekData = shallowRef()
const loadContentWeekData = async () => {
  let result = await proxy.Request({
    url: api.getContentWeekData,
    dataType: 'json',
    method: 'get',
  })
  if (!result) return

  const data = result.result
  const xAxisData = data.dataList
  const seriesData = []
  const colors = ['#1b9cfc', '#67c23a', '#33166e', '#fb7993', '#a4e4fc']
  data.itemDataList.forEach((item, index) => {
    seriesData.push({
      name: item.statisticsName,
      type: 'bar',
      data: item.listData,
      itemStyle: {
        color: colors[index],
      },
    })
  })
  myChartContentWeekData.value.setOption(getOption('内容统计', xAxisData, seriesData))
}

const initAppWeekData = () => {
  nextTick(() => {
    myChartAppWeekData.value = echarts.init(myChartAppWeekDataRef.value)
    myChartContentWeekData.value = echarts.init(myChartContentWeekDataRef.value)
    loadAppWeekData()
    loadContentWeekData()
  })
}

loadAllData()
initAppWeekData()
</script>

<style lang="scss" scoped>
.card-title {
  font-weight: bold;
  font-size: 20px;
  margin-bottom: 10px;
}
.patr-panel {
  margin-top: 10px;
  &:first-child {
    margin-top: 0px;
  }
}
.data-list {
  .data-item {
    background: #f4f9fd;
    color: #9a9fa6;
    padding: 20px;
    border-radius: 5px;
    width: 100%;
    .data-panel {
      display: flex;
      align-items: center;
      justify-content: space-between;
    }
    .data {
      font-size: 25px;
      color: #000012;
      font-weight: bold;
      margin-top: 10px;
    }
    .pre {
      margin-top: 5px;
      .new {
        color: #ff6873;
      }
    }
  }
}
.chart {
  height: calc(100vh - 400px);
}
</style>
