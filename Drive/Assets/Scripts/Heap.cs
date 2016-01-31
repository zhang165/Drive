using UnityEngine;
using System.Collections;
using System;

public class Heap<E> where E: IHeapItem<E>{
	E[] items;
	int count;

	public Heap(int max){
		items = new E[max];
	}

	public void Add(E item){
		item.HeapIndex = count;
		items [count] = item;
		sortUp (item);
		count++;
	}

	public E pop(){
		E firstItem = items [0];
		count--;
		items [0] = items [count];
		items [0].HeapIndex = 0;
		sortDown (items [0]);
		return firstItem;
	}

	void sortUp(E item){
		int parentIndex = (item.HeapIndex - 1) / 2;
		while (true) {
			E parentItem = items [parentIndex];
			if (item.CompareTo (parentItem) > 0) {
				swap (item, parentItem);
			} else {
				break;
			}
			parentIndex = (item.HeapIndex - 1) / 2;
		}
	}

	void sortDown(E item){
		while (true) {
			int left = item.HeapIndex * 2 + 1;
			int right = item.HeapIndex * 2 + 2;
			int swapIndex = 0;
			if (left < count) {
				swapIndex = left;
				if (right < count) {
					if (items [left].CompareTo (items [right]) < 0) {
						swapIndex = right;
					}
				}
				if (item.CompareTo (items [swapIndex]) < 0) {
					swap (item, items [swapIndex]);
				} else {
					return;
				}
			} else {
				return;
			}
		}
	}

	public bool contains(E item){
		return Equals (items [item.HeapIndex], item);
	}

	public void UpdateItem(E item){
		sortUp (item);
	}


	public int Count{
		get{
			return count;
		}
	}

	void swap(E itemA, E itemB){
		items [itemA.HeapIndex] = itemB;
		items[itemB.HeapIndex] = itemA;
		int temp = itemA.HeapIndex;
		itemA.HeapIndex = itemB.HeapIndex;
		itemB.HeapIndex = temp;
	}
}

public interface IHeapItem<E>: IComparable<E>{
	int HeapIndex {
		get;
		set;
	}
}