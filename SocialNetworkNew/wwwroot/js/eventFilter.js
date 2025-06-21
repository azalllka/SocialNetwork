document.addEventListener("DOMContentLoaded", function () {
    const dateFilters = document.querySelectorAll(".hidden-date-filter");

    dateFilters.forEach((filter) => {
        filter.addEventListener("change", function () {
            const selectedFilter = this.value;

            // ��������� URL � ��������� ��������
            const url = new URL(window.location.href);
            url.searchParams.set("dateFilter", selectedFilter);

            // �������������� ������������ �� ����������� URL
            window.location.href = url.toString();
        });
    });
});