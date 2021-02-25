<template>
    <el-dialog
        :visible.sync="show"
        :close-on-click-modal="false"
        title="设置角色"
        width="40%"
        top="10vh"
        scrollable
    >
        <div
            style="max-height: 550px; overflow-x: auto;"
            class="form-container"
        >
            <el-checkbox
                v-model="checkAll"
                :indeterminate="isIndeterminate"
                @change="handleCheckAllChange"
            >
                全选
            </el-checkbox>
            <div style="margin: 15px 0;" />
            <el-checkbox-group
                v-model="checkedRoles"
                size="small"
                @change="handleCheckedCitiesChange"
            >
                <el-checkbox
                    v-for="item in roleList"
                    :key="item.roleId"
                    :label="item.roleId"
                    border
                    style="margin-bottom: 10px;"
                >
                    {{ item.roleName }}
                </el-checkbox>
            </el-checkbox-group>
        </div>
        <div slot="footer" class="dialog-footer">
            <el-button @click="close">取 消</el-button>
            <el-button type="primary" @click="ok">确 定</el-button>
        </div>
    </el-dialog>
</template>
<script>
/**
 * 用户添加编辑
 */
export default {
    components: {},
    data() {
        return {
            show: false,
            loading: false,
            checkAll: false,
            isIndeterminate: true,
            checkedRoles: [],
            roleList: [],
            userId: null
        }
    },
    mounted() {},
    methods: {
        handleCheckAllChange(val) {
            this.checkedRoles = val ? this.roleList.map(r => r.roleId) : []
            this.isIndeterminate = false
        },
        handleCheckedCitiesChange(value) {
            let checkedCount = value.length
            this.checkAll = checkedCount === this.roleList.length
            this.isIndeterminate =
                checkedCount > 0 && checkedCount < this.roleList.length
        },
        loadRoles() {
            this.$api.get('api/system/getallroles').then(res => {
                this.roleList = res.data
            })
        },
        // 确定
        ok: function() {
            this.loading = true
            let _data = {
                userId: this.userId,
                roleId: this.checkedRoles
            }
            this.$api
                .post('api/system/usersetrole', _data)
                .then(res => {
                    if (res.succeeded) {
                        this.$message.success('角色设置成功')
                        this.$emit('on-ok') // 成功响应事件
                        this.close()
                    } else {
                        this.$message.error(res.errors)
                    }
                })
                .catch(() => (this.loading = false))
        },
        // 关闭窗口
        close: function() {
            this.loading = false
            this.show = false
        },
        // 打开窗口
        open: function(row) {
            this.show = true
            this.checkedRoles = []
            this.checkAll = false
            this.isIndeterminate = true
            this.userId = row.userId
            if (row.roleId) {
                this.checkedRoles = row.roleId
            }
            this.loadRoles()
        }
    }
}
</script>
<style>
.form-editor {
    padding-right: 5px;
}
.form-container > .el-checkbox.is-bordered.el-checkbox--small {
    margin-bottom: 15px;
}
</style>
