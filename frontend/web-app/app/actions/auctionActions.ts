'use server'

import { Auction, PagedResult } from "@/types";

export const getData = async (pageNumber: number, pageSize: number): Promise<PagedResult<Auction>> => {
    const res = await fetch(`http://localhost:6001/search?pageSize=${pageSize}&pageNumber=${pageNumber}`);

    if (!res.ok) {
        throw new Error("Failed to fetch data");
    }

    return res.json();
}