<template>
    <el-dialog
        :visible.sync="show"
        :close-on-click-modal="false"
        :title="title"
        width="35%"
        top="10vh"
        scrollable
    >
        <div style="max-height: 550px; overflow-x: auto;" class="form-container">
            <el-form ref="form" :model="formData" :rules="formRules" label-width="120px">
                <el-form-item label="自定义汇率" prop="customRate">
                    <el-input-number
                        v-model="formData.customRate"
                        :precision="4"
                        :step="0.1"
                        placeholder="请输入自定义汇率"
                        clearable
                        style="width: 100%;"
                    />
                </el-form-item>

                <el-form-item label="汇损比例(%)" prop="exchangeLoss">
                    <el-input-number
                        v-model="formData.exchangeLoss"
                        :precision="2"
                        :step="0.1"
                        :min="1"
                        :max="100"
                        placeholder="请输入汇损比例"
                        clearable
                        style="width: 100%;"
                    />
                </el-form-item>
            </el-form>
        </div>
        <div slot="footer" class="dialog-footer">
            <el-button @click="close">取 消</el-button>
            <el-button type="primary" @click="ok">确 定</el-button>
        </div>
    </el-dialog>
</template>
<script>
// import { addEdit } from "@/api/sys/role";
/**
 * 用户添加编辑
 */
export default {
    components: {},
    data() {
        return {
            show: false,
            loading: false,
            // 表单数据
            formData: {
                exchangeRateId: null,
                customRate: null,
                exchangeLoss: null
            },
            formRules: {
                customRate: [
                    { required: true, message: '请输入自定义汇率', trigger: 'blur' }
                ],
                exchangeLoss: [
                    { required: true, message: '请输入汇损比例', trigger: 'blur' }
                ]
            }
        }
    },
    computed: {
        title() {
            return this.formData.userId ? '编辑' : '添加'
        }
    },
    mounted() {},
    methods: {
        // 确定
        ok: function() {
            this.$refs.form.validate(valid => {
                if (valid) {
                    // this.loading = true;
                    // save
                    this.$message.success('验证成功')
                }
            })
        },
        // 关闭窗口
        close: function() {
            this.loading = false
            this.show = false
        },
        // 打开窗口
        open: function(row) {
            this.show = true
            this.$nextTick(() => {
                this.$refs.form.resetFields()
                this.formData.exchangeRateId = null
                if (row) {
                    this.formData.exchangeRateId = row.exchangeRateId
                    this.formData.customRate = row.customRate
                    this.formData.exchangeLoss = row.exchangeLoss
                }
            })
        }
    }
}
</script>
<style>
.form-editor {
    padding-right: 5px;
}
</style>
