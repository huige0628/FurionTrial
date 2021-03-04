<template>
    <div class="page-main">
        <!-- head -->
        <SimpleHead
            :model="filter"
            @on-search="reloadTable(true)"
            @on-expand="onExpand"
        >
            <template slot="button">
                <!-- 操作按钮 -->
                <el-form-item>
                    <el-button-group>
                        <el-button
                            type="primary"
                            icon="el-icon-edit"
                            @click="setExchangeLoss"
                        >
                            编辑
                        </el-button>
                    </el-button-group>
                </el-form-item>
            </template>
            <!-- 默认搜索项 -->
            <el-form-item>
                <el-input
                    v-model="filter.name"
                    placeholder="货币名称"
                    clearable
                />
            </el-form-item>
            <template slot="more">
                <!-- 更多搜索项 -->
            </template>
        </SimpleHead>
        <!-- table -->
        <SimpleTable ref="table" :url="tableUrl" :filter="filter" :offset="10">
            <el-table-column type="selection" width="40" fixed="left" />
            <el-table-column
                prop="currencyName"
                label="货币名称"
            />
            <el-table-column
                prop="currencyCode"
                label="货币代码"
            />
            <el-table-column prop="rateUsd" label="汇率(USD)" />
            <el-table-column prop="rateCny" label="汇率(CNY)" />
            <el-table-column prop="rateHkd" label="汇率(HKD)" />
            <el-table-column
                prop="customRate"
                label="自定义汇率"
            />
            <el-table-column
                prop="exchangeLoss"
                label="汇损比例(%)"
            />
            <el-table-column
                prop="modifyDate"
                label="最后更新时间"
            />
        </SimpleTable>
        <!-- 编辑汇损 -->
        <SetExchangeLoss ref="setExchangeLoss" @on-ok="reloadTable" />
    </div>
</template>
<script>
export default {
    name: 'Currency',
    components: {
        SetExchangeLoss: () => import('./setexchangeloss') // 编辑汇损
    },
    data() {
        return {
            tableUrl: `${process.env.VUE_APP_API_ROOT}api/baseinfo/getcurrencylist`,
            // 过滤项
            filter: {
                name: null,
                code: null
            }
        }
    },
    methods: {
        reloadTable(again) {
            this.$refs.table.reload(again)
        },
        onExpand() {
            this.$refs.table.resize()
        },
        setExchangeLoss() {
            this.$refs.setExchangeLoss.open()
        }
    }
}
</script>
