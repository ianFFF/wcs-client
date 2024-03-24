<template>
  <div>
    <a-modal v-model="visible" :title="title" @ok="handleOk">
      <template slot="footer">
        <a-button key="cancel" @click="handleOk('cancel')">取消</a-button>
        <a-button key="submit" type="primary" :loading="loading" @click="handleOk('submit')">确定</a-button>
      </template>
      <a-spin tip="Loading..." :spinning="loading">
        <a-form-model
          ref="ruleForm"
          :model="form"
          :rules="rules"
          :label-col="labelCol"
          :wrapper-col="wrapperCol"
          :loading="true"
        >
          <a-form-model-item label="货架编号" prop="shelves_code">
            <a-input v-model="form.shelves_code" placeholder="请填写货架编号" />
          </a-form-model-item>
          <a-form-model-item label="货架列数" prop="shelves_cols_num">
            <a-input v-model="form.shelves_cols_num" placeholder="请填写货架列数" type="number" addon-after="列" />
          </a-form-model-item>
          <a-form-model-item label="货架层数" prop="shelves_rows_num">
            <a-input v-model="form.shelves_rows_num" placeholder="请填写货架层数" type="number" addon-after="层" />
          </a-form-model-item>
          <a-form-model-item label="货架深度" prop="shelves_deep_num">
            <a-input v-model="form.shelves_deep_num" placeholder="请填写货架深度" type="number" addon-after="深" />
          </a-form-model-item>
          <a-form-model-item label="单格最大承重量" prop="max_load">
            <a-input v-model="form.max_load" placeholder="请填写单格最大承重量" type="number" addon-after="KG" />
          </a-form-model-item>
          <a-form-model-item label="库区" prop="warehouse_area">
            <a-select v-model="form.warehouse_area" placeholder="请选择设备所属库区">
              <a-select-option v-for="item in warehouseAreaList" :key="item.value" :value="item.value">{{
                item.label
              }}</a-select-option>
            </a-select>
          </a-form-model-item>
          <a-form-model-item label="上游系统ID" prop="upper_system_id">
            <a-input v-model="form.upper_system_id" placeholder="上游系统ID" disabled />
          </a-form-model-item>
        </a-form-model>
      </a-spin>
    </a-modal>
  </div>
</template>
<script>
export default {
  name: 'EquipmentForm',

  data() {
    return {
      visible: false,
      loading: false,
      title: '货架录入',

      labelCol: { span: 6 },
      wrapperCol: { span: 16 },
      form: {
        id: '',
        shelves_code: '',
        shelves_cols_num: '',
        shelves_rows_num: '',
        shelves_deep_num: '',
        max_load: '',
        upper_system_id: '',
        warehouse_area: undefined
      },
      rules: {
        shelves_code: [{ required: true, message: '请填写货架编号' }],
        shelves_cols_num: [{ required: true, message: '请填写货架列数' }],
        shelves_rows_num: [{ required: true, message: '请填写货架层数' }],
        shelves_deep_num: [{ required: true, message: '请填写货架深度' }],
        max_load: [{ required: true, message: '请填写货架单格最大承重量' }]
      },

      warehouseAreaList: [
        {
          label: '库区A',
          value: '1'
        },
        {
          label: '库区B',
          value: '2'
        }
      ]
    }
  },
  methods: {
    showModal(id) {
      console.log('showModal' + id)
      this.title = id ? '货架编辑' : '货架录入'
      this.form.id = id
      this.visible = true
      if (this.$refs.ruleForm) this.$refs.ruleForm.resetFields()
      this.getDefaultData()
    },
    handleOk(e) {
      if (e === 'cancel') {
        this.visible = false
        return
      }

      this.$refs.ruleForm.validate((valid) => {
        if (valid) {
          this.loading = true
          setTimeout(() => {
            this.visible = false
            this.loading = false
            this.$message.success('操作成功')
            this.$emit('onFinish', { type: e, data: this.form })
          }, 3000)
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    getDefaultData() {
      if (!this.form.id) return
      this.loading = true

      setTimeout(() => {
        const result = {
          id: '1',
          shelves_code: 'HJ0005',
          shelves_cols_num: '5',
          shelves_rows_num: '3',
          shelves_deep_num: '1',
          max_load: '11',
          upper_system_id: '',
          warehouse_area: '2'
        }
        this.form = { ...result }

        this.loading = false

        console.log('finish getDefaultData')
      }, 2000)
    }
  }
}
</script>
