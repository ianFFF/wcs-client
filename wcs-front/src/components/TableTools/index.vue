<template>
  <div class="table-tools">
    <div class="tools-before">
      <slot name="ex-before"></slot>
      <a-input-search v-if="needSearch" placeholder="请输入关键词搜索" style="width: 224px" @search="onSearch" allowClear/>
    </div>
    <div class="tools-btn">
      <a-button class="btn" v-if="needAdd" type="primary" @click="onAdd">新增</a-button>
      <a-button class="btn" v-if="needExcel" type="primary" @click="onExcel">导出</a-button>
      <slot name="ex-after"></slot>
    </div>
  </div>
</template>

<script>
export default {
  name: 'TableTools',
  props: {
    /**
     * ['search','add','excel']
     */
    actions: {
      type: Array,
      default: null
    }
  },
  data() {
    return {}
  },
  computed: {
    needSearch() {
      if (!this.actions) return true
      return this.actions.indexOf('search') > -1
    },
    needAdd() {
      if (!this.actions) return true
      return this.actions.indexOf('add') > -1
    },
    needExcel() {
      if (!this.actions) return true
      return this.actions.indexOf('excel') > -1
    }
  },
  methods: {
    onSearch(val) {
      this.$emit('onSearch', val)
    },
    onAdd() {
      this.$emit('onAdd')
    },
    onExcel() {
      this.$emit('onExcel')
    }
  }
}
</script>

<style lang="less" scoped>
.table-tools {
  display: flex;
  justify-content: space-between;
  .btn{
    margin-right: 8px;
  }
}
</style>
